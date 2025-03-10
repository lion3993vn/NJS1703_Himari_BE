using HimariServer.Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.OrderModels
{
    public class OrderUpdateModel
    {
        public int OrderId { get; set; }
        public string Address { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; }
    }
}
