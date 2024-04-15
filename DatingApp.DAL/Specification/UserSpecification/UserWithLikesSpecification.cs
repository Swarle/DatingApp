using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.UserSpecification;

public sealed class UserWithLikesSpecification : BaseSpecification<AppUser>
{
    public UserWithLikesSpecification(int id) : base(e => e.Id == id)
    {
        AddInclude(e => e.LikedUsers);
    }   
}