using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IDeepseekService
    {
        Task<string> ResponseMessage(string userText);
        Task<string> FormatMessageUser(string userText);
        
        Task StreamResponseMessage(string userText, Func<string, Task> onMessageReceived);
    }
}
