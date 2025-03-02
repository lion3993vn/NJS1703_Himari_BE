using HimariServer.Repository.Enums;
using System;

namespace HimariServer.Service.BusinessModels.ChatMessageModels
{
    public class ChatMessageModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public MessageType Type { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
