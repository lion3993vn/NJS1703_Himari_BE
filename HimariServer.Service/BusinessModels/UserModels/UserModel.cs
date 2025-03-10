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

        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string AddressBonus { get; set; }

        public int? Point { get; set; }

        public string Status { get; set; }

        public string? Role { get; set; }
    }
}
