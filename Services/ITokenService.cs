using Nail_Service.Models;

namespace Nail_Service.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
