using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.LikeSpecification;

public sealed class LikeBySourceAndTargetUserIdSpecification : BaseSpecification<UserLike>
{
    public LikeBySourceAndTargetUserIdSpecification(int sourceId, int targetId)
        : base(e => e.SourceUserId == sourceId && e.TargetUserId == targetId)

    {

    }
}