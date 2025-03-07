using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.UserModels
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string UnsignName { get; set; }

        public string Email { get; set; }

        public string GoogleId { get; set; }

        public string? AvatarUrl { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int? Point { get; set; }

        public string Status { get; set; }

        public string? Role { get; set; }
    }
}
