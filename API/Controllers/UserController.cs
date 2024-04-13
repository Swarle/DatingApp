using DatingApp.BL.DTO;
using DatingApp.BL.Extensions;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams param)
        {
            var users = await _service.GetAllUsersAsync(param);

            return Ok(users);
        }
        
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            var user = await _service.GetUserByUsernameAsync(username);

            return Ok(user);
        }
        
        [HttpPut]
        public async Task<ActionResult> UpdateUserAsync(MemberUpdateDto memberDto)
        {
            await _service.UpdateUserAsync(memberDto);

            return NoContent();
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhotoAsync(IFormFile file)
        {
            var photoDto = await _service.AddPhotoAsync(file);

            return CreatedAtAction(nameof(GetUser), 
                new { username = User.GetUsername() }, photoDto);
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhotoAsync(int photoId)
        {
            await _service.SetMainPhotoAsync(photoId);

            return NoContent();
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            await _service.DeletePhotoAsync(photoId);

            return Ok();
        }
    }
}
