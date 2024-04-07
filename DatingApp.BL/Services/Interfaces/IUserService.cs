using DatingApp.BL.DTO;

namespace DatingApp.BL.Services.Interfaces;

public interface IUserService
{
    public Task<IEnumerable<MemberDto>> GetAllUsersAsync();
    public Task<MemberDto> GetUserByUsernameAsync(string username);
    public Task UpdateUserAsync(MemberUpdateDto memberDto);

}