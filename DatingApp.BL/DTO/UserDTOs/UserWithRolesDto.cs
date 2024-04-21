namespace DatingApp.BL.DTO.UserDTOs;

public class UserWithRolesDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required List<string> Roles { get; set; }
}