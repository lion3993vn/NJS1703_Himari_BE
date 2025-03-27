using HimariServer.Repository.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.OrderModels
{
    public class OrderRequestModel
    {
        public int UserId { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public List<ItemCreateOrder> Items { get; set; }
    }

    public class ItemCreateOrder
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
