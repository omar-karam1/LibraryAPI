using CityPulse.Helpers;
using Library.Models;
using Library.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace Library.Repository
{
    public class UserRep:IUserRep
    {
        AppDbContext db = new AppDbContext();
        AuthServiceModel authModel = new AuthServiceModel();
        private readonly AppDbContext dbContext;
        private readonly AuthService _authService;

        public UserRep(AppDbContext dbContext, AuthService authService)
        {
            this.dbContext = dbContext;
            _authService = authService;

        }


        public async Task<string> SaveProfileImageAsync(IFormFile profileImage)
        {
            if (profileImage == null)
                return "/images/User/profile_user.jpg";

            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(profileImage.FileName)}";
            var filePath = Path.Combine("wwwroot/images/User", fileName);


            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }


            return $"/images/User/{fileName}";
        }


        public async Task<bool> IsEmailUnique(string email)
        {
            if (dbContext.Users.Any(u => u.Email == email))
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsUserNameUnique(string username)
        {
            if (dbContext.Users.Any(u => u.UserName == username))
            {
                return false;
            }
            return true;
        }

        public async Task<AuthServiceModel> RegisterAsync(RegisterModel model)
        {
            if (!await IsEmailUnique(model.Email))
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "Email is already taken";
                return authModel;
            }

            if (!await IsUserNameUnique(model.UserName))
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "userName is already taken";
                return authModel;
            }




            var user = new User
            {
                Name = model.Name,
                UserName = model.UserName,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
               BirthDate=model.BirthDate

            };

            authModel.IsAuthenticated = true;
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return authModel;


        }

        public async Task<AuthServiceModel> Login(LoginModel loginModel)
        {

            var user = await dbContext.Users
                     .FirstOrDefaultAsync(u => u.UserName == loginModel.Username);



            if (user != null && BCrypt.Net.BCrypt.Verify(loginModel.Password, user.Password))
            {
                authModel.IsAuthenticated = true;
                authModel.Token = _authService.GenerateJwtToken(user);
                authModel.Roles = user.UserType;
                authModel.id = user.UserId;
                return authModel;

            }
            else
            {
                authModel.IsAuthenticated = false;
                authModel.Message = "Invalid username or password";
                return authModel;
            }


        }

        public User? GetUserInformaionById(int id)
        {
            return dbContext.Users.FirstOrDefault(u => u.UserId == id);
        }


        public async Task<bool> EditUserData(int id, string name, IFormFile image)
        {



            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
            if (user == null)
                return false;


            user.Name = name;
            if (image != null)
            {
                string profileImagePath = await SaveProfileImageAsync(image);
                user.ProfileImage = profileImagePath;
            }

            await dbContext.SaveChangesAsync();
            return true;

        }

    }
}
