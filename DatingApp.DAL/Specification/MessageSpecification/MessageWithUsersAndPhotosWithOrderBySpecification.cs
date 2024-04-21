using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.MessageSpecification;

public sealed class MessageWithUsersAndPhotosWithOrderBySpecification : BaseSpecification<Message>
{
    public MessageWithUsersAndPhotosWithOrderBySpecification(string currentUsername, string recipientUsername, bool isDescendingOrderBy = false)
    : base(m => m.RecipientUsername == currentUsername &&
                m.RecipientDeleted == false &&
                m.SenderUsername == recipientUsername ||
                m.RecipientUsername == recipientUsername &&
                m.SenderDeleted == false &&
                m.SenderUsername == currentUsername)
    {
        AddInclude(m => m.Recipient);
        AddInclude(m => m.Sender);
        AddInclude($"{nameof(Message.Recipient)}.{nameof(AppUser.Photos)}");
        AddInclude($"{nameof(Message.Sender)}.{nameof(AppUser.Photos)}");
        AddOrderBy(m => m.MessageSent);
        IsDescendingOrderBy = isDescendingOrderBy;
    }
}