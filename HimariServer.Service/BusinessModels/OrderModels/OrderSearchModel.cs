using HimariServer.Repository.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.OrderModels
{
    public class OrderSearchModel
    {
        public int? OrderCode { get; set; }
        public string? UserUnsignName { get; set; }
        public string? Address { get; set; }
        public PaginationParameter PaginationParameter { get; set; } = new PaginationParameter();
    }
}
