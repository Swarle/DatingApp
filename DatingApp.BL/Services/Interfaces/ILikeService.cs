using DatingApp.BL.DTO.LikeDTOs;
using DatingApp.BL.Infrastructure;
using DatingApp.DAL.Entities;

namespace DatingApp.BL.Services.Interfaces;

public interface ILikeService
{
    Task<IEnumerable<LikeDto>> GetUserLikes(LikesParams likesParams);
    Task AddLikeAsync(string username);
}