using DatingApp.DAL.Entities;
using DatingApp.DAL.Specification.Infrastructure;

namespace DatingApp.DAL.Specification.MessageSpecification;

public sealed class MessageByContainerWithUsersAndPhotosWithOrderBySpecification : BaseSpecification<Message>
{
    public MessageByContainerWithUsersAndPhotosWithOrderBySpecification(string container, string username)
    {
        AddInclude(m => m.Recipient);
        AddInclude(m => m.Sender);
        AddInclude($"{nameof(Message.Recipient)}.{nameof(AppUser.Photos)}");
        AddInclude($"{nameof(Message.Sender)}.{nameof(AppUser.Photos)}");
        
        switch (container)
        {
            case "Inbox":
                AddExpression(m => m.RecipientUsername == username && m.RecipientDeleted == false);
                break;
            case "Outbox":
                AddExpression(m => m.SenderUsername == username && m.SenderDeleted == false);
                break;
            default:
                AddExpression(m => m.RecipientUsername == username && m.RecipientDeleted == false
                                   && m.DateRead == null);
                break;
        }
        
        AddOrderBy(m => m.MessageSent);
    }
    
    
}