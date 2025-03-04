using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.SettingModels
{
    public class MomoSettings
    {
        public string BaseUrl { get; set; }
        public string SecretKey { get; set; }
        public string AccessKey { get; set; }
        public string RedirectUrl { get; set; }  // Updated field
        public string IpnUrl { get; set; }       // Updated field
        public string PartnerCode { get; set; }
        public bool AutoCapture { get; set; }
        public string Lang { get; set; }
    }
}
