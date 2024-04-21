namespace DatingApp.BL.Infrastructure;

public class MessageParams : PaginationParams
{
    public string Container { get; set; } = "Unread";
}