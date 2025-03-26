using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using HimariServer.Repository.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Repositories.Interfaces
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        Task<Order?> GetOrderByCodeAsync(int orderCode);
        Task<int> GetTotalOrder(int? month, int? year);
        Task<int> GetNotStartedOrder(int? month, int? year);
        Task<int> GetOrderByDeliveryStatus(int? month, int? year, DeliveryStatus status);
        Task<int> GetTotalPriceByMonth(int month);
        Task<int> GetTotalPriceByMonthAndYear(int month, int year);
        Task<int> GetTotalPriceWithDeliveryStatus(DeliveryStatus status);
        Task<int> GetTotalPriceWithPaymentStatus(PaymentStatus status);
    }
}
