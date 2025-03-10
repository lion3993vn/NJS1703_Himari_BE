using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HimariServer.Service.BusinessModels.UserModels
{
    public class UpdateUserModel
    {
        public required int Id { get; set; }
        public required string Email { get; set; }
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string AddressBonus { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
