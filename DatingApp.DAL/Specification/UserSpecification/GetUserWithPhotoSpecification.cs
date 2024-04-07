using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.UserSpecification;

public sealed class GetUserWithPhotoSpecification : BaseSpecification<AppUser>
{
    public GetUserWithPhotoSpecification() : base()
    {
        AddInclude(e => e.Photos);
    }
}