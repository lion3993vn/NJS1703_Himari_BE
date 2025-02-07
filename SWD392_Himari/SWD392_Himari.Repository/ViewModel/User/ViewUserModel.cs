using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD392_Himari.Repository.ViewModel.User
{
    public class ViewUserModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }
        public string avatar { get; set; }
        public string Code { get; set; }
        public bool isVerify { get; set; }
        public string Email { get; set; }

        public string NumberPhone { get; set; }

        public string Password { get; set; }

        public string AccessToken { get; set; }
    }
}
