namespace DatingApp.BL.DTO.MessagesDTOs;

public class CreateMessageDto
{
    public required string RecipientUsername { get; set; }
    public required string Content { get; set; }
}