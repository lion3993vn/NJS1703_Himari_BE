using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.UserDeviceModels
{
    public class UserDeviceModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string DeviceToken { get; set; } = string.Empty;
    }
}
