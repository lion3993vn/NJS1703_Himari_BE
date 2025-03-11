using HimariServer.Repository.Enums;
using HimariServer.Repository.UnitOfWork;
using HimariServer.Service.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Implements
{
    public class PaymentPeriodicService : BackgroundService, IPaymentPeriodicService
    {
        private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(1));
        private readonly IServiceScopeFactory _scopeFactory;

        public PaymentPeriodicService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task UpdatePaymentInfo()
        {
            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var payOSService = scope.ServiceProvider.GetRequiredService<IPayOSService>();

                    var listPaymentPending = await unitOfWork.PaymentRepository.GetPaymentPending();
                    if (!listPaymentPending.Any()) return;

                    foreach (var item in listPaymentPending)
                    {
                        var paymentInfo = await payOSService.GetPaymentInfo(item.Order.OrderCode);

                        if (paymentInfo.status == "EXPIRED")
                        {
                            Console.WriteLine("cap nhat status cho don hang: " + item.Order.OrderCode);
                            item.Status = PaymentStatus.Failed;
                            unitOfWork.PaymentRepository.UpdateAsync(item);
                        }
                    }

                    unitOfWork.Save();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(stoppingToken))
                {
                    await UpdatePaymentInfo();
                }
            }
            catch (OperationCanceledException)
            {

            }
        }
    }
}
