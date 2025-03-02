using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.ChatMessageModels
{
    public class ChatResponseModel
    {
        public string Content { get; set; }
        public List<int>? Products { get; set; }
    }
}
