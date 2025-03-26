using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.DashboardModels
{
    public class LowQuantityProductModel
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
    }
}
