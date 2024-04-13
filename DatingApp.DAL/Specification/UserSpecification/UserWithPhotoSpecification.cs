using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.UserSpecification;

public sealed class UserWithPhotoSpecification : BaseSpecification<AppUser>
{
    public UserWithPhotoSpecification() : base()
    {
        AddInclude(e => e.Photos);
    }
    public UserWithPhotoSpecification(string username) : base(u => u.UserName == username.ToLower())
    {
        AddInclude(e => e.Photos);
    }
}