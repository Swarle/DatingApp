using DatingApp.BL.DTO;

namespace DatingApp.BL.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<UserDto> RegisterAsync(RegisterDto registerDto);
    }
}
