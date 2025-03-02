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

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseResponseModel> CreateOrder(OrderResquestModel model)
        {
            // Check if user exists
            var user = await _unitOfWork.UsersRepository.GetByIdAsync(model.UserId);
            if (user == null)
            {
                throw new NotExistException(MessageConstants.USER_NOT_EXIST);
            }

            // Validate order items
            if (model.Items == null || !model.Items.Any())
            {
                throw new DefaultException(MessageConstants.ORDER_ITEM_NOT_HAVE);
            }

            string orderCode = StringUtils.GenerateOrderCode(5);

            // Create order entity
            var order = new Order
            {
                UserId = model.UserId,
                OrderCode = orderCode,
                OrderPrice = 0,
                CreatedDate = DateTime.Now,
                IsDeleted = false
            };

            // Add order to database
            await _unitOfWork.OrderRepository.AddAsync(order);
            await _unitOfWork.SaveAsync();

            // Calculate total amount and create order details
            double totalAmount = 0;

            foreach (var item in model.Items)
            {
                // Get product from database
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    throw new NotExistException(MessageConstants.ORDER_ITEM_NOT_FOUND.Replace("{id}", item.ProductId.ToString()));
                }

                if (product.Quantity < item.Quantity)
                {
                    throw new DefaultException(MessageConstants.INSUFFICIENT_STOCK_QUANTITY.Replace("{name}", product.ProductName));
                }

                // Calculate item price
                var itemPrice = product.Price * item.Quantity;
                totalAmount += itemPrice ?? 0;

                // Create order detail
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
            }

            order.OrderPrice = totalAmount;
            _unitOfWork.OrderRepository.UpdateAsync(order);

            await _unitOfWork.SaveAsync();

            // Return response
            return null;
        }
    }
}
