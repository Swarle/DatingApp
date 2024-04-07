using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.UserSpecification;

public sealed class GetUserWithPhotosByUsernameSpecification : BaseSpecification<AppUser>
{
    public GetUserWithPhotosByUsernameSpecification(string username) : base(u => u.UserName == username.ToLower())
    {
        AddInclude(e => e.Photos);
    }
}