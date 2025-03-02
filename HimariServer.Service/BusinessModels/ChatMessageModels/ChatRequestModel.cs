using HimariServer.Repository.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.ChatMessageModels
{
    public class ChatRequestModel
    {
        public string Message { get; set; }
        public int? UserId { get; set; }
    }
}
