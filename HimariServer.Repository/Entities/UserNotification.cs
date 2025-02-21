using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Entities
{
    public partial class UserNotification : BaseEntity
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; } = false;
        public virtual Notification Notification { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
