using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.ChatMessageModels
{
    public class APIChatResponseModel
    {
        public string ChatMessage { get; set; }
        public Keywords Keywords { get; set; }
    }
    public class Keywords
    {
        public string BodyPart { get; set; }
        public string Symptom { get; set; }
    }
}
