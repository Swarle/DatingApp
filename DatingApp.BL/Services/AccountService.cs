

using System.Net;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using DatingApp.BL.DTO;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services.Interfaces;
using DatingApp.DAL.Entities;
using DatingApp.DAL.Repository.Interfaces;
using DatingApp.DAL.Specification.UserSpecification;

namespace DatingApp.BL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IRepository<AppUser> _repository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountService(IRepository<AppUser> repository, IMapper mapper, ITokenService tokenService)
        {
            _repository = repository;
            _mapper = mapper;
            _tokenService = tokenService;
        }


        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            if (await IsUserExist(registerDto.Username))
                throw new HttpException(HttpStatusCode.BadRequest, "Username is taken");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            await _repository.CreateAsync(user);
            await _repository.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var userSpecification = new FindUserByUsernameSpecification(loginDto.Username);

            var user = await _repository.FindSingle(userSpecification);

            if (user == null) throw new HttpException(HttpStatusCode.Unauthorized, "Invalid username");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    throw new HttpException(HttpStatusCode.Unauthorized, "Invalid password");
            }

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user),
            };
        }

        private async Task<bool> IsUserExist(string username)
        {
            var userSpecification = new FindUserByUsernameSpecification(username);

            return await _repository.FindAny(userSpecification);
        }
    }
}

