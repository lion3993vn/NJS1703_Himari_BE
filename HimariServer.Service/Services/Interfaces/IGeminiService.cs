using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.Services.Interfaces
{
    public interface IGeminiService
    {
        Task StreamResponseMessage(string userText, Func<string, Task> onMessageReceived);

        Task<string> IntentMessage(string userText);
    }
}
