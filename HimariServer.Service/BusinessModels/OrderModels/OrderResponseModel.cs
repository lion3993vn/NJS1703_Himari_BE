using HimariServer.Repository.Entities;
using HimariServer.Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.OrderModels
{
    public class OrderResponseModel
    {
        public int Id { get; set; }
        public int OrderCode { get; set; }
        public int OrderPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public int DeliveryStatus { get; set; } // Changed from string to int
        public PaymentStatus PaymentStatus { get; set; }
        public List<OrderDetailsModel> OrderDetails { get; set; }
    }
    public class OrderDetailsModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
    /*
         var orderList = orders.Select(order => new
            {
                order.Id,
                order.OrderCode,
                order.OrderPrice,
                order.CreatedDate,
                order.Payments,
                Products = order.OrderDetails.Select(od => new
                {
                    od.ProductId,
                    od.Product.ProductName,
                    od.Quantity,
                    od.Price
                }).ToList()
            }).ToList();*/
}
