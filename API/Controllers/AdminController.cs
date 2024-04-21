using System.Net;
using DatingApp.BL.DTO.UserDTOs;
using DatingApp.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Utility;

namespace API.Controllers
{
    [Authorize]
    public class AdminController : BaseApiController
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService _adminService)
        {
            this._adminService = _adminService;
        }
        
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("users-with-roles")]
        public async Task<ActionResult<UserWithRolesDto>> GetUsersWithRoles()
        {
            var users = await _adminService.GetUsersWithRolesAsync();

            return Ok(users);
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("edit-roles/{username}")]
        public async Task<ActionResult> EditRolesAsync(string username, [FromQuery] string roles)
        {
            var rolesDto = await _adminService.EditRolesAsync(username, roles);

            return Ok(rolesDto);
        }
    }
}
