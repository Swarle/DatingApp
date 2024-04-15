using DatingApp.BL.DTO.LikeDTOs;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class LikesController : BaseApiController
{
    private readonly ILikeService _likeService;

    public LikesController(ILikeService likeService)
    {
        _likeService = likeService;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddLike(string username)
    {
        await _likeService.AddLikeAsync(username);

        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes([FromQuery]LikesParams likesParam)
    {
        var likesDto = await _likeService.GetUserLikes(likesParam);

        return Ok(likesDto);
    }
}