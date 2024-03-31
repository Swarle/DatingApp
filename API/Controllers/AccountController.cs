using DatingApp.BL.DTO;
using DatingApp.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> RegisterAsync(RegisterDto registerDto)
        {
            throw new Exception("oops");
            var userDto = await _accountService.RegisterAsync(registerDto);

            return userDto;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> LoginAsync(LoginDto loginDto)
        {
            var userDto = await _accountService.LoginAsync(loginDto);

            return userDto;
        }
    }
}
