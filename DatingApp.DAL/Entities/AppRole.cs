using Microsoft.AspNetCore.Identity;

namespace DatingApp.DAL.Entities;

public class AppRole : IdentityRole<int>
{
    public ICollection<AppUserRole> UserRoles { get; set; } = null!;
}