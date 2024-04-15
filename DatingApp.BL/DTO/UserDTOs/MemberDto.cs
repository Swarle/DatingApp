namespace DatingApp.BL.DTO;

public class MemberDto
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public string? PhotoUrl { get; set; }
    public int Age { get; set; }
    public required string KnownAs { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastActive { get; set; }
    public required string Gender { get; set; }
    public required string Introduction { get; set; }
    public required string LookingFor { get; set; }
    public required string Interests { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public required ICollection<PhotoDto> Photos { get; set; }
}