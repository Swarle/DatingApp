namespace DatingApp.BL.DTO;

public class MemberUpdateDto
{
    public required string Introduction { get; set; }
    public required string LookingFor { get; set; }
    public string Interests { get; set; } = null!;
    public required string City { get; set; }
    public required string Country { get; set; }
    
}