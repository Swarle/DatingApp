using DatingApp.BL.DTO.MessagesDTOs;
using DatingApp.BL.Infrastructure;
using DatingApp.DAL.Infrastructure;

namespace DatingApp.BL.Services.Interfaces;

public interface IMessageService
{
    public Task<MessageDto> AddMessageAsync(CreateMessageDto createMessageDto);
    public Task<IEnumerable<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams);
    public Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string username);
    public Task DeleteMessageAsync(int id);
}