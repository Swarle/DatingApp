namespace DatingApp.BL.Infrastructure;

public class LikesParams : PaginationParams
{
    public required string Predicate { get; set; }
}