namespace DatingApp.DAL.Entities;

public class Message
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public AppUser Sender { get; set; } = null!;
    public string? SenderUsername { get; set; }
    public int RecipientId { get; set; }
    public AppUser Recipient { get; set; } = null!;
    public string? RecipientUsername { get; set; }
    public string? Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.Now;
    public bool SenderDeleted { get; set; }
    public bool RecipientDeleted { get; set; }
}