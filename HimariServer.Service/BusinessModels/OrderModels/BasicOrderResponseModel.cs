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
    public class BasicOrderResponseModel
    {
        public int Id { get; set; }
        public int OrderCode { get; set; }
        public int OrderPrice { get; set; }
        public DateTime CreatedDate { get; set; }
        public DeliveryStatus DeliveryStatus { get; set; } // Changed from string to int
        public PaymentStatus PaymentStatus { get; set; }
        public string Address { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
    }
}
