using DatingApp.BL.DTO;
using DatingApp.BL.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace DatingApp.BL.Services.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<MemberDto>> GetAllUsersAsync(UserParams param);
    public Task<MemberDto> GetUserByUsernameAsync(string username);
    public Task UpdateUserAsync(MemberUpdateDto memberDto);
    public Task<PhotoDto> AddPhotoAsync(IFormFile file);
    public Task SetMainPhotoAsync(int photoId);
    public Task DeletePhotoAsync(int photoId);

}