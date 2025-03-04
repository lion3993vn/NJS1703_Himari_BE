using HimariServer.Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.OrderModels
{
    public class OrderResquestModel
    {
        public int UserId { get; set; }
        public string Address { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<ItemCreateOrder> Items { get; set; }
    }

    public class ItemCreateOrder
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
