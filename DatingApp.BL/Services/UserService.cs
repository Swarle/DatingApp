using System.Net;
using System.Security.Claims;
using AutoMapper;
using DatingApp.BL.DTO;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services.Interfaces;
using DatingApp.DAL.Entities;
using DatingApp.DAL.Repository.Interfaces;
using DatingApp.DAL.Specification.UserSpecification;
using Microsoft.AspNetCore.Http;

namespace DatingApp.BL.Services;

public class UserService : IUserService
{
    private readonly IRepository<AppUser> _repository;
    private readonly HttpContext _httpContext; 
    private readonly IMapper _mapper;

    public UserService(IRepository<AppUser> repository, IMapper mapper, IHttpContextAccessor accessor)
    {
        _repository = repository;
        _mapper = mapper;
        _httpContext = accessor.HttpContext ?? throw new InvalidOperationException("HttpContextAccessor does`t have context");
    }
    public async Task<IEnumerable<MemberDto>> GetAllUsersAsync()
    {
        var specification = new GetUserWithPhotoSpecification();
        
        var users = await _repository.Find(specification);

        var usersDto = _mapper.Map<IEnumerable<MemberDto>>(users);

        return usersDto;
    }

    public async Task<MemberDto> GetUserByUsernameAsync(string username)
    {
        var specification = new GetUserWithPhotosByUsernameSpecification(username);

        var user = await _repository.FindSingle(specification) ?? 
                   throw new HttpException(HttpStatusCode.BadRequest, $"No user with Username: \"{username}\"");

        var userDto = _mapper.Map<MemberDto>(user);

        return userDto;
    }

    public async Task UpdateUserAsync(MemberUpdateDto memberDto)
    {
        var username = _httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (username == null) throw new InvalidOperationException("Username claim is empty");

        var specification = new GetUserWithPhotosByUsernameSpecification(username);

        var user = await _repository.FindSingle(specification);

        if (user == null) throw new HttpException(HttpStatusCode.NotFound, $"No user with Username: \"{username}\"");

        _mapper.Map(memberDto, user);

        await _repository.Update(user);
        await _repository.SaveChangesAsync();
    }
}