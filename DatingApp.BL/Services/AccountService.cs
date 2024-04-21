using System.Net;
using AutoMapper;
using DatingApp.BL.DTO;
using DatingApp.BL.DTO.UserDTOs;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services.Interfaces;
using DatingApp.DAL.Entities;
using DatingApp.DAL.Repository.Interfaces;
using DatingApp.DAL.Specification.UserSpecification;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.BL.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IRepository<AppUser> _repository;
        private readonly IMapper _mapper;

        public AccountService(UserManager<AppUser> userManager,IRepository<AppUser> repository, IMapper mapper, ITokenService tokenService)
        {

            _userManager = userManager;
            _repository = repository;
            _mapper = mapper;
            _tokenService = tokenService;
        }


        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await IsUserExist(registerDto.Username))
                throw new HttpException(HttpStatusCode.BadRequest, "Username is taken");
            
            var user = _mapper.Map<AppUser>(registerDto);


            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                throw new HttpException(HttpStatusCode.BadRequest, result.Errors.ToString());

            return new UserDto
            {
                Username = user.UserName!,
                Token = _tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var userSpecification = new UserWithPhotoSpecification(loginDto.Username);

            var user = await _repository.GetFirstOrDefaultAsync(userSpecification);

            if (user == null) throw new HttpException(HttpStatusCode.Unauthorized, "Invalid username");

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
                throw new HttpException(HttpStatusCode.Unauthorized, "Invalid password");
            
            return new UserDto
            {
                Username = user.UserName!,
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };
        }

        private async Task<bool> IsUserExist(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
        }
    }
}

