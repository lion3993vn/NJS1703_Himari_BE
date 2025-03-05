using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.Constants;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.SettingModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class PayOSService : IPayOSService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly PayOSSettings _payOSSettings;
        public PayOSService(IOptions<PayOSSettings> payOSSettings, IUnitOfWork unitOfWork)
        {
            _payOSSettings = payOSSettings.Value;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> CreatePaymentUrl(int orderId)
        {
            PayOS payOS = new PayOS(_payOSSettings.ClientID, _payOSSettings.ApiKey, _payOSSettings.ChecksumKey);

            var order = await _unitOfWork.OrderRepository.GetByIdIncludeAsync(
                orderId,
                include: query => query.Include(o => o.OrderDetails).ThenInclude(od => od.Product)
                );

            List<ItemData> items = new List<ItemData>();
            foreach (var orderDetail in order.OrderDetails)
            {
                ItemData item = new ItemData(
                    orderDetail.Product.ProductName, 
                    (int)orderDetail.Quantity, 
                    (int)(orderDetail.Quantity * orderDetail.Price));
                items.Add(item);
            }

            var payment = await _unitOfWork.PaymentRepository.GetByOrderIdAsync(orderId);
            
            long expiredAt = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 600;

            var paymentData = new PaymentData(
                order.OrderCode,
                (int)payment.Amount,
                MessageConstants.PAYMENT_DESCRIPTION + order.OrderCode,
                items,
                _payOSSettings.CancelUrl,
                _payOSSettings.ReturnUrl,
                null,null,null,null,null,
                expiredAt);

            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
            return createPayment.checkoutUrl;
        }

        public WebhookData VerifyWebhook(WebhookType webhook)
        {
            PayOS payOS = new PayOS(_payOSSettings.ClientID, _payOSSettings.ApiKey, _payOSSettings.ChecksumKey);

            return payOS.verifyPaymentWebhookData(webhook);
        }
    }
}
