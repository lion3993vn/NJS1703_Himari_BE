using HimariServer.Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.NotificationModels
{
    public class SystemNotificationModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? TitleUnsign { get; set; }
        public string? Message { get; set; }
        public string? Href { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
