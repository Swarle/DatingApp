using System.Net;
using AutoMapper;
using DatingApp.BL.DTO;
using DatingApp.BL.Extensions;
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
    private readonly IPhotoService _photoService;

    public UserService(IRepository<AppUser> repository, IMapper mapper, IHttpContextAccessor accessor,
        IPhotoService photoService)
    {
        _repository = repository;
        _mapper = mapper;
        _httpContext = accessor.HttpContext ?? throw new InvalidOperationException("HttpContextAccessor does`t have context");
        _photoService = photoService;
    }
    public async Task<IEnumerable<MemberDto>> GetAllUsersAsync(UserParams param)
    {
        var currentUsername = _httpContext.User.GetUsername() ??
                 throw new HttpException(HttpStatusCode.Unauthorized);

        var userSpecification = new UserByUsernameSpecification(currentUsername);

        var currentUser = await _repository.GetFirstOrDefaultAsync(userSpecification, false) ??
                          throw new HttpException(HttpStatusCode.NotFound, $"No user with Username: {currentUsername} ");

        if (string.IsNullOrEmpty(param.Gender))
            param.Gender = currentUser.Gender == "male" ? "female" : "male";
        
        var specification = new UserWithPhotoAndFilteringSpecification(currentUsername,param.Gender,
            param.MaxAge, param.MinAge, param.OrderBy!);
        
        var users = await _repository.GetPagedCollectionAsync(specification,
            param.PageNumber, param.PageSize);

        if (!users.Any())
            throw new HttpException(HttpStatusCode.NotFound);
        
        _httpContext.Response.AddPaginationHeader(users.CurrentPage,
            users.PageSize, users.TotalCount, users.TotalPages);

        var usersDto = _mapper.Map<IEnumerable<MemberDto>>(users);

        return usersDto;
    }

    public async Task<MemberDto> GetUserByUsernameAsync(string username)
    {
        var user = await GetUserAsync(username);

        var userDto = _mapper.Map<MemberDto>(user);

        return userDto;
    }

    public async Task UpdateUserAsync(MemberUpdateDto memberDto)
    {
        var user = await GetUserAsync();

        _mapper.Map(memberDto, user);

        await _repository.Update(user);
        await _repository.SaveChangesAsync();
    }

    public async Task<PhotoDto> AddPhotoAsync(IFormFile file)
    {
        var user = await GetUserAsync();

        var uploadResult = await _photoService.AddPhotoAsync(file);

        if (uploadResult.Error != null) 
            throw new HttpException(HttpStatusCode.BadRequest, uploadResult.Error.Message);

        var photo = _mapper.Map<Photo>(uploadResult);

        if (user.Photos.Count == 0)
            photo.IsMain = true;
        
        user.Photos.Add(photo);

        await _repository.SaveChangesAsync();

        var photoDto = _mapper.Map<PhotoDto>(photo);

        return photoDto;
    }

    public async Task SetMainPhotoAsync(int photoId)
    {
        var user = await GetUserAsync();

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId) ??
                    throw new HttpException(HttpStatusCode.NotFound);

        if (photo.IsMain) 
            throw new HttpException(HttpStatusCode.BadRequest, "This is already your main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

        if (currentMain != null)
            currentMain.IsMain = false;

        photo.IsMain = true;

        await _repository.SaveChangesAsync();
    }

    public async Task DeletePhotoAsync(int photoId)
    {
        var user = await GetUserAsync();

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId) ??
                    throw new HttpException(HttpStatusCode.NotFound);

        if (photo.IsMain)
            throw new HttpException(HttpStatusCode.BadRequest, "You cannot delete your main photo");

        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);

            if (result.Error != null)
                throw new HttpException(HttpStatusCode.BadRequest, result.Error.Message);
        }

        user.Photos.Remove(photo);

        await _repository.SaveChangesAsync();
    }


    private async Task<AppUser> GetUserAsync(string? username = null)
    {
        username ??= _httpContext.User.GetUsername() ??
                     throw new InvalidOperationException("Username claim is empty");
        
        var specification = new UserWithPhotoSpecification(username);

        var user = await _repository.GetFirstOrDefaultAsync(specification);

        if (user == null) 
            throw new HttpException(HttpStatusCode.NotFound, $"No user with Username: \"{username}\"");

        return user;
    }
}