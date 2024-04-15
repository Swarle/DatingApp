using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.UserSpecification;

public sealed class UserByPredicateSpecification : BaseSpecification<AppUser>
{
    public UserByPredicateSpecification(string predicate, int userId)
    {
        AddInclude(u => u.LikedUsers);
        AddInclude(u => u.LikedByUsers);
        AddInclude(u => u.Photos);

        switch (predicate)
        {
            case "liked":
                AddExpression(u => u.LikedByUsers.Any(like => like.SourceUserId == userId));
                break;
            case "likedBy":
                AddExpression(u => u.LikedUsers.Any(like => like.TargetUserId == userId));
                break;
            default:
                throw new InvalidOperationException("Predicate should contains value \"liked\" or \"likedBy\"");
        }
    }
}