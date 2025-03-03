using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.PayOSModels
{
    public class PayOSRequest
    {
        public int Amount { get; set; }
        public string Description { get; set; }
        public List<ItemData> Items { get; set; }
        public string? CancelUrl { get; set; }
        public string? ReturnUrl { get; set; }
        public int? ExpiredAt { get; set; }
    }
}
