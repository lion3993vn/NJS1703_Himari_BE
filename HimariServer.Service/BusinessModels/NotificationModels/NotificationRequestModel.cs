using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.NotificationModels
{
    public class NotificationRequestModel
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
