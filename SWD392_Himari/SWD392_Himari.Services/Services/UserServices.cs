using Backend_reactNative_Shoppee_Services.Interfaces;
using SWD392_Himari.Repository.Interfaces;
using SWD392_Himari.Repository.ViewModel.User;

namespace Backend_reactNative_Shoppee_Services.Services
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository userRepository;

        public UserServices(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<IEnumerable<ViewUserModel>> getAllUser()
        {
            return await userRepository.getAllUser();
        }

        public async Task<ViewUserModel> getUserById(string userId)
        {
            return await userRepository.getUserById(userId);
        }
    }
}
