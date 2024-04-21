using Microsoft.AspNetCore.Identity;

namespace DatingApp.DAL.Entities;

public class AppUserRole : IdentityUserRole<int>
{
    public required AppUser User { get; set; }
    public required AppRole Role { get; set; }
}