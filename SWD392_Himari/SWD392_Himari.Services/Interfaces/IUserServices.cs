using SWD392_Himari.Repository.ViewModel.User;

namespace Backend_reactNative_Shoppee_Services.Interfaces
{
    public interface IUserServices
    {
        Task<IEnumerable<ViewUserModel>> getAllUser();
        Task<ViewUserModel> getUserById(string userId);


    }
}
