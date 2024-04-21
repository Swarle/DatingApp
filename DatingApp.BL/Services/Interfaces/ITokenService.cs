

using DatingApp.DAL.Entities;

namespace DatingApp.BL.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(AppUser user);
    }
}
