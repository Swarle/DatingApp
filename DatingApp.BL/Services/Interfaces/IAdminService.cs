using DatingApp.BL.DTO.UserDTOs;

namespace DatingApp.BL.Services.Interfaces;

public interface IAdminService
{
    public Task<IEnumerable<UserWithRolesDto>> GetUsersWithRolesAsync();
    public Task<IEnumerable<string>> EditRolesAsync(string username, string roles);
}