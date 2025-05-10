using Library.Models;
using Library.ViewModel;

namespace Library.Repository
{
    public interface IUserRep
    {
        Task<AuthServiceModel> RegisterAsync(RegisterModel model);
        Task<AuthServiceModel> Login(LoginModel loginModel);
        Task<string> SaveProfileImageAsync(IFormFile profileImage);
        Task<bool> IsUserNameUnique(string username);
        Task<bool> IsEmailUnique(string username);
        User? GetUserInformaionById(int id);
        Task<bool> EditUserData(int id, string username, IFormFile image);
    }
}
