using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.UserSpecification;

public sealed class UserWithRolesSpecification : BaseSpecification<AppUser>
{
    public UserWithRolesSpecification()
    {
        AddInclude($"{nameof(AppUser.UserRoles)}");
        AddInclude($"{nameof(AppUser.UserRoles)}.{nameof(AppUserRole.Role)}");
        AddOrderBy(u => u.UserName);
    }
}