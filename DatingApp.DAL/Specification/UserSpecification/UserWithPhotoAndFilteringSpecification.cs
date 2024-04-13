using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.UserSpecification;

public sealed class UserWithPhotoAndFilteringSpecification : BaseSpecification<AppUser>
{
    public UserWithPhotoAndFilteringSpecification(string username,
        string gender, int maxAge, int minAge, string orderBy) : base(u => u.UserName != username)
    {
        AddInclude(u => u.Photos);
        AddExpression(u => u.Gender == gender);

        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-maxAge - 1));
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-minAge));

        AddExpression(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);
        AddOrderBy(orderBy switch
        {
            "created" => u => u.Created,
            _ => u => u.LastActive
        });
    }
}