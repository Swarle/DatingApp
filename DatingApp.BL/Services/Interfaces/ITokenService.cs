

using DatingApp.DAL.Entities;

namespace DatingApp.BL.Services.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user);
    }
}
