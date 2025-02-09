using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.AuthenModels
{
    public class AuthenModel
    {
        public string AccessToken { get; set; } = "";

        public string RefreshToken { get; set; } = "";
    }
}
