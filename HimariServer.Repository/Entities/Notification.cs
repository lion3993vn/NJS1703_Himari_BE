using HimariServer.Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Entities
{
    public partial class Notification : BaseEntity
    {
        public string? Title { get; set; }
        public string? TitleUnsign { get; set; }
        public string? Message { get; set; }
        public string? Href { get; set; }
        public NotificationType? Type { get; set; }
    }
}
