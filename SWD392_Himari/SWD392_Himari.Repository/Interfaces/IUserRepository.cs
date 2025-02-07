using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD392_Himari.Repository.ViewModel.User;

namespace SWD392_Himari.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<ViewUserModel>> getAllUser();
        Task<ViewUserModel> getUserById(string userId);
        

    }
}
