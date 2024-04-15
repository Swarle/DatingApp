using System.Net;
using AutoMapper;
using DatingApp.BL.DTO.LikeDTOs;
using DatingApp.BL.Extensions;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services.Interfaces;
using DatingApp.DAL.Entities;
using DatingApp.DAL.Repository.Interfaces;
using DatingApp.DAL.Specification.LikeSpecification;
using DatingApp.DAL.Specification.UserSpecification;
using Microsoft.AspNetCore.Http;

namespace DatingApp.BL.Services;

public class LikeService : ILikeService
{
    private readonly IRepository<UserLike> _likeRepository;
    private readonly IRepository<AppUser> _userRepository;
    private readonly IMapper _mapper;
    private readonly HttpContext _httpContext;

    public LikeService(IRepository<UserLike> likeRepository, IRepository<AppUser> userRepository,
        IMapper mapper, IHttpContextAccessor accessor)
    {
        _likeRepository = likeRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _httpContext = accessor.HttpContext 
                       ?? throw new InvalidOperationException("HttpContextAccessor does`t have context");
    }

    public async Task<IEnumerable<LikeDto>> GetUserLikes(LikesParams likesParams)
    {
        var userId = _httpContext.User.GetUserId() ??
                     throw new HttpException(HttpStatusCode.Unauthorized);

        var userSpecification = new UserByPredicateSpecification(likesParams.Predicate, userId);
        var pagedUsers = await _userRepository.GetPagedCollectionAsync(userSpecification,
            likesParams.PageNumber, likesParams.PageSize);
        
        _httpContext.Response.AddPaginationHeader(pagedUsers.CurrentPage, pagedUsers.PageSize,
            pagedUsers.TotalCount, pagedUsers.TotalPages);

        var likesDto = _mapper.Map<IEnumerable<LikeDto>>(pagedUsers);

        return likesDto;
    }

    public async Task AddLikeAsync(string username)
    {
        var sourceUserId = _httpContext.User.GetUserId() ??
                           throw new HttpException(HttpStatusCode.Unauthorized);

        var userByUsernameSpecification = new UserByUsernameSpecification(username);
        var likedUser = await _userRepository.GetFirstOrDefaultAsync(userByUsernameSpecification) ?? 
                        throw new HttpException(HttpStatusCode.NotFound);

        var userWithLikesSpecification = new UserWithLikesSpecification(sourceUserId);
        var sourceUser = await _userRepository.GetFirstOrDefaultAsync(userWithLikesSpecification) ??
                         throw new HttpException(HttpStatusCode.NotFound);

        if (sourceUser.UserName == username)
            throw new HttpException(HttpStatusCode.BadRequest, "You cannot like yourself");

        var likeSpecification = new LikeBySourceAndTargetUserIdSpecification(sourceUserId, likedUser.Id);
        var isUserLiked = await _likeRepository.IsSatisfiedAsync(likeSpecification);

        if (isUserLiked)
            throw new HttpException(HttpStatusCode.BadRequest, "You already like this user");

        var userLike = new UserLike
        {
            SourceUserId = sourceUserId,
            TargetUserId = likedUser.Id
        };
        
        sourceUser.LikedUsers.Add(userLike);

        await _userRepository.SaveChangesAsync();
    }
}