using AutoMapper;
using HimariServer.Repository.Commons;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.EmailModels;
using HimariServer.Service.BusinessModels.OrderModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace HimariServer.Service.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPayOSService _payOSService;
        private readonly IMailService _mailService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IPayOSService payOSService, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOSService = payOSService;
            _mailService = mailService;
        }

        public async Task<BaseResponseModel> CreateOrder(OrderRequestModel model)
        {
            #region create order

            int totalAmount = 0;
            foreach (var item in model.Items)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new NotExistException(MessageConstants.ORDER_ITEM_NOT_FOUND.Replace("{id}", item.ProductId.ToString()));
                }

                if (product.Quantity < item.Quantity)
                {
                    throw new DefaultException(MessageConstants.INSUFFICIENT_STOCK_QUANTITY.Replace("{name}", product.ProductName));
                }

                var itemPrice = product.Price * item.Quantity;
                totalAmount += itemPrice ?? 0;
            }

            var user = await _unitOfWork.UsersRepository.GetByIdAsync(model.UserId);
            if (user == null)
            {
                throw new NotExistException(MessageConstants.USER_NOT_EXIST);
            }

            if (model.Items == null || !model.Items.Any())
            {
                throw new DefaultException(MessageConstants.ORDER_ITEM_NOT_HAVE);
            }

            int orderCode = await ValidateOrderCode();

            var order = new Order
            {
                UserId = model.UserId,
                OrderCode = orderCode,
                OrderPrice = 0,
                Address = model.Address,
                DeliveryStatus = DeliveryStatus.NotStarted,
            };

            // Add order to database
            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveAsync();

            foreach (var item in model.Items)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);

                var itemPrice = product.Price * item.Quantity;

                var orderDetail = new OrderDetail
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = product.Price
                };

                await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);

                // Update product quantity
                product.Quantity -= item.Quantity;
                _unitOfWork.ProductRepository.UpdateAsync(product);

                ItemData itemPayment = new ItemData(product.ProductName, item.Quantity, (int)itemPrice);
            }

            order.OrderPrice = totalAmount;
            _unitOfWork.OrderRepository.UpdateAsync(order);
            #endregion

            #region create payment
            var payment = new Payment
            {
                OrderId = order.Id,
                Amount = totalAmount,
                Description = MessageConstants.PAYMENT_DESCRIPTION + order.OrderCode,
                PaymentMethod = model.PaymentMethod,
                Status = PaymentStatus.Pending,
            };

            await _unitOfWork.PaymentRepository.AddAsync(payment);

            await _unitOfWork.SaveAsync();
            #endregion

            #region handle payment payos
            if (model.PaymentMethod == PaymentMethod.PayOS)
            {
                var paymenturl = await _payOSService.CreatePaymentUrl(order.Id);

                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = MessageConstants.ORDER_CREATE_SUCCESS,
                    Data = new
                    {
                        OrderCode = order.OrderCode,
                        PaymentUrl = paymenturl
                    }
                };
            }
            #endregion

            #region handle payment momo
            else
            {
                return new BaseResponseModel
                {
                    StatusCode = StatusCodes.Status200OK,
                    Message = MessageConstants.ORDER_CREATE_SUCCESS,
                    Data = new
                    {
                        OrderCode = order.OrderCode
                    }
                };
            }
            #endregion
        }

        private async Task<int> ValidateOrderCode()
        {
            while (true)
            {
                var orderCode = GenerateOrderCode();
                if (await _unitOfWork.OrderRepository.GetOrderByCodeAsync(orderCode) == null)
                {
                    return orderCode;
                }
            }
        }

        private int GenerateOrderCode()
        {
            Random random = new();
            return int.Parse((DateTimeOffset.Now.ToUnixTimeSeconds() % 10000000).ToString() + random.NextInt64(1, 10));
        }

        public async Task ConfirmOrderPayment(WebhookType webhook)
        {
            var data = _payOSService.VerifyWebhook(webhook);

            var order = await _unitOfWork.OrderRepository.GetOrderByCodeAsync((int)data.orderCode);

            var payment = await _unitOfWork.PaymentRepository.GetByOrderIdAsync(order.Id);

            if (data.code == "00")
            {
                payment.Status = PaymentStatus.Success;
                _unitOfWork.PaymentRepository.UpdateAsync(payment);
                order.DeliveryStatus = DeliveryStatus.Preparing;
                _unitOfWork.OrderRepository.UpdateAsync(order);

                _ = Task.Run(async () =>
                    await _mailService.SendEmailAsync(new MailRequest()
                    {
                        Subject = $"Himari - Đơn hàng mới {(int)data.orderCode}",
                        Body = EmailUtils.OrderMail(_mapper.Map<OrderResponseModel>(order)),
                        ToEmail = order.User.Email,
                    }));
            }
            else
            {
                payment.Status = PaymentStatus.Failed;

                foreach (var item in payment.Order.OrderDetails)
                {
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync((int)item.ProductId);
                    product.Quantity += item.Quantity;
                    _unitOfWork.ProductRepository.UpdateAsync(product);
                }

                _unitOfWork.PaymentRepository.UpdateAsync(payment);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task<BaseResponseModel> GetOrderByUserId(int userId, PaginationParameter paginationParameter)
        {
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new NotExistException(MessageConstants.USER_NOT_EXIST);
            }

            var orders = await _unitOfWork.OrderRepository.ToPaginationIncludeAsync(
                paginationParameter,
                filter: x => x.UserId == userId,
                include: query => query.Include(o => o.OrderDetails)
                                      .ThenInclude(od => od.Product)
                                      .Include(o => o.Payments),
                orderBy: query => query.OrderByDescending(x => x.CreatedDate)
            );

            // Map Order entities to OrderResponseModel using AutoMapper
            var orderResponseList = new List<OrderResponseModel>();
            foreach (var order in orders)
            {
                var orderResponse = _mapper.Map<OrderResponseModel>(order);

                // Get payment status
                var payment = order.Payments?.FirstOrDefault();
                if (payment != null)
                {
                    orderResponse.PaymentStatus = payment.Status;
                }

                // Convert enum to string for DeliveryStatus
                orderResponse.DeliveryStatus = order.DeliveryStatus;


                orderResponseList.Add(orderResponse);
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.GET_LIST_ORDER_SUCCESS,
                Data = new ModelPaging
                {
                    Data = orderResponseList,
                    MetaData = new
                    {
                        orders.TotalCount,
                        orders.PageSize,
                        orders.CurrentPage,
                        orders.TotalPages,
                        orders.HasNext,
                        orders.HasPrevious
                    }
                }
            };
        }

        public async Task<BaseResponseModel> UpdateOrder(OrderUpdateModel orderUpdateModel)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(orderUpdateModel.OrderId);
            if (order == null)
            {
                throw new NotExistException(MessageConstants.ORDER_NOT_FOUND);
            }

            // Update order properties
            order.Address = orderUpdateModel.Address;
            order.DeliveryStatus = orderUpdateModel.DeliveryStatus;

            _unitOfWork.OrderRepository.UpdateAsync(order);
            await _unitOfWork.SaveAsync();

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.ORDER_UPDATE_SUCCESS,
                Data = new
                {
                    OrderId = order.Id,
                    OrderCode = order.OrderCode,
                    Address = order.Address,
                    DeliveryStatus = (int)order.DeliveryStatus
                }
            };
        }

        public async Task<BaseResponseModel> GetOrderByOrderCode(int orderCode)
        {
            var order = await _unitOfWork.OrderRepository.GetOrderByCodeAsync(orderCode);

            if (order == null)
            {
                throw new NotExistException(MessageConstants.ORDER_NOT_FOUND);
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.ORDER_FOUND,
                Data = _mapper.Map<OrderResponseModel>(order)
            };
        }

        public async Task<BaseResponseModel> GetAllOrders(PaginationParameter paginationParameter)
        {
            var orders = await _unitOfWork.OrderRepository.ToPaginationIncludeAsync(
                     paginationParameter,
                     include: query => query.Include(o => o.Payments),
                     orderBy: query => query.OrderByDescending(x => x.CreatedDate)
                 );

            if (orders == null)
            {
                throw new NotExistException(MessageConstants.ORDER_NOT_FOUND);
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.ORDER_FOUND,
                Data = _mapper.Map<Pagination<BasicOrderResponseModel>>(orders)
            };
        }

        public async Task<BaseResponseModel> GetOrderByOrderId(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdIncludeAsync(orderId,
                include: query => query.Include(o => o.Payments));

            if (order == null)
            {
                throw new NotExistException(MessageConstants.ORDER_NOT_FOUND);
            }

            return new BaseResponseModel
            {
                StatusCode = StatusCodes.Status200OK,
                Message = MessageConstants.ORDER_FOUND,
                Data = _mapper.Map<BasicOrderResponseModel>(order)
            };
        }
    }
}
