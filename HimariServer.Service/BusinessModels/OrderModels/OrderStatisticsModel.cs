using HimariServer.Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace HimariServer.Service.BusinessModels.OrderModels
{
    public class OrderStatisticsModel
    {
        public int TotalOrder { get; set; }
        public int NotStartedOrder { get; set; }
        public int PreparingOrder { get; set; }
        public int DeliveringOrder { get; set; }
        public int DeliveredOrder { get; set; }
        public int CancelledOrder { get; set; }

    }
}
