using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.SettingModels
{
    public class PayOSSettings
    {
        public required string ClientID { get; set; }
        public required string ApiKey { get; set; }
        public required string ChecksumKey { get; set; }
        public required string CancelUrl { get; set; }
        public required string ReturnUrl { get; set; }
    }
}
