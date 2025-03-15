using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.NotificationModels
{
    public class NotificationRequestModel
    {
        public string Title { get; set; }
        public string Message { get; set; }
    }

    public class NotificationRequestUserModel
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
    public class MarkAllAsReadRequest
    {
        public int UserId { get; set; }
    }
}
