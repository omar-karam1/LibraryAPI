
using Library.Models;

namespace CityPulse.Helpers
{
    public interface IAuthService
    {
       string GenerateJwtToken(User user);
    }
}
