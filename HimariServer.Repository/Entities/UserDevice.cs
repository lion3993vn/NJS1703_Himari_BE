using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Repository.Entities
{
    public partial class UserDevice : BaseEntity
    {
        public int UserId { get; set; }

        public string DeviceToken { get; set; } = string.Empty;

        public virtual User User { get; set; }
    }
}
