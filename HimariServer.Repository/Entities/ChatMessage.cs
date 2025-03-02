using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HimariServer.Repository.Enums;

namespace HimariServer.Repository.Entities
{
    public partial class ChatMessage : BaseEntity
    {
        public string Message { get; set; }
        public MessageType Type { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
