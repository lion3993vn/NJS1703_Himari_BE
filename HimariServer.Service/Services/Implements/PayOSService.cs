using HimariServer.Service.BusinessModels.PayOSModels;
using HimariServer.Service.Services.Interfaces;
using HimariServer.Service.SettingModels;
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
        private readonly PayOSSettings _payOSSettings;
        public PayOSService(IOptions<PayOSSettings> payOSSettings)
        {
            _payOSSettings = payOSSettings.Value;
        }

        public async Task<string> CreatePaymentUrl(PayOSRequest request)
        {
            long orderCode = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            PayOS payOS = new PayOS(_payOSSettings.ClientID, _payOSSettings.ApiKey, _payOSSettings.ChecksumKey);

            var paymentData = new PaymentData(
                    orderCode, 
                    request.Amount, 
                    request.Description, 
                    request.Items, 
                    _payOSSettings.CancelUrl, 
                    _payOSSettings.ReturnUrl);

            CreatePaymentResult createPayment = await payOS.createPaymentLink(paymentData);
            return createPayment.checkoutUrl;
        }
    }
}
