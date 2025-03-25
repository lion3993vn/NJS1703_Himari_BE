using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.DashboardModels
{
    public class RevenueModel
    {
        public string Revenue { get; set; }
        public string Percent { get; set; }
        public bool IsIncrease { get; set; }
    }
}
