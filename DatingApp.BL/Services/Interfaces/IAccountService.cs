using DatingApp.BL.DTO;
using DatingApp.BL.DTO.UserDTOs;

namespace DatingApp.BL.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<UserDto> RegisterAsync(RegisterDto registerDto);

        public Task<UserDto> LoginAsync(LoginDto loginDto);
    }
}
