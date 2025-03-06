using AutoMapper;
using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.BusinessModels.OrderModels;
using HimariServer.Service.BusinessModels.ResultModels;
using HimariServer.Service.Constants;
using HimariServer.Service.Exceptions;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.Utils;
using Microsoft.AspNetCore.Http;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPayOSService _payOSService;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IPayOSService payOSService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _payOSService = payOSService;
        }

        public async Task<BaseResponseModel> CreateOrder(OrderResquestModel model)
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
            };

            // Add order to database
            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveAsync();

            foreach (var item in model.Items)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);

                var itemPrice = product.Price * item.Quantity;
                totalAmount += itemPrice ?? 0;

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
            Random random = new Random();
            return random.Next(100000, 1000000);
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
            }
            else
            {
                payment.Status = PaymentStatus.Failed;

                _unitOfWork.PaymentRepository.UpdateAsync(payment);
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
