using System.Collections;
using System.Net;
using AutoMapper;
using DatingApp.BL.DTO.MessagesDTOs;
using DatingApp.BL.Extensions;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services.Interfaces;
using DatingApp.DAL.Entities;
using DatingApp.DAL.Infrastructure;
using DatingApp.DAL.Repository.Interfaces;
using DatingApp.DAL.Specification.MessageSpecification;
using DatingApp.DAL.Specification.UserSpecification;
using Microsoft.AspNetCore.Http;
using Utility;

namespace DatingApp.BL.Services;

public class MessageService : IMessageService
{
    private readonly IRepository<Message> _messageRepository;
    private readonly IRepository<AppUser> _userRepository;
    private readonly HttpContext _httpContext;
    private readonly IMapper _mapper;

    public MessageService(IRepository<Message> messageRepository, IMapper mapper, IRepository<AppUser> userRepository,
    IHttpContextAccessor accessor)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
        _userRepository = userRepository;
        _httpContext = accessor.HttpContext 
                       ?? throw new InvalidOperationException("HttpContextAccessor does`t have context");
    }

    public async Task<MessageDto> AddMessageAsync(CreateMessageDto createMessageDto)
    {
        var currentUsername = _httpContext.User.GetUsername() ?? 
                              throw new InvalidOperationException(SD.InvalidOperationMessage);

        if (currentUsername == createMessageDto.RecipientUsername.ToLower())
            throw new HttpException(HttpStatusCode.BadRequest, "You cannot send messages to yourself");

        var senderUserSpecification = new UserWithPhotoSpecification(currentUsername);
        var sender = await _userRepository.GetFirstOrDefaultAsync(senderUserSpecification) ??
                     throw new HttpException(HttpStatusCode.BadRequest, $"User with username: \"{currentUsername}\" doest not exist");

        var recipientUserSpecification = new UserWithPhotoSpecification(createMessageDto.RecipientUsername);
        var recipient = await _userRepository.GetFirstOrDefaultAsync(recipientUserSpecification) ??
                        throw new HttpException(HttpStatusCode.NotFound);

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = createMessageDto.Content
        };

        await _messageRepository.CreateAsync(message);

        await _messageRepository.SaveChangesAsync();

        var messageDto = _mapper.Map<MessageDto>(message);

        return messageDto;
    }

    public async Task<IEnumerable<MessageDto>> GetMessagesForUserAsync(MessageParams messageParams)
    {
        var currentUsername = _httpContext.User.GetUsername() ??
                              throw new InvalidOperationException(SD.InvalidOperationMessage);

        var messageSpecification =
            new MessageByContainerWithUsersAndPhotosWithOrderBySpecification(messageParams.Container, currentUsername);

        var pagedMessages = await _messageRepository.GetPagedCollectionAsync(messageSpecification, messageParams.PageNumber,
            messageParams.PageSize);
        
        _httpContext.Response.AddPaginationHeader(pagedMessages.CurrentPage, pagedMessages.PageSize,
            pagedMessages.TotalCount, pagedMessages.TotalPages);

        var messagesDto = _mapper.Map<IEnumerable<MessageDto>>(pagedMessages);

        return messagesDto;
    }

    public async Task<IEnumerable<MessageDto>> GetMessageThreadAsync(string username)
    {
        var currentUsername = _httpContext.User.GetUsername() ??
                              throw new InvalidOperationException(SD.InvalidOperationMessage);

        var messageSpecification = new MessageWithUsersAndPhotosWithOrderBySpecification(currentUsername, username);

        var messages = await _messageRepository.GetAllAsync(messageSpecification);

        var unreadMessages = messages.Where(m => m.DateRead == null &&
                      m.RecipientUsername == currentUsername)
            .ToList();

        if (unreadMessages.Any())
        {
            foreach (var message in unreadMessages)
            {
                message.DateRead = DateTime.UtcNow;
            }

            await _messageRepository.SaveChangesAsync();
        }

        var messagesDto = _mapper.Map<IEnumerable<MessageDto>>(messages);

        return messagesDto;
    }

    public async Task DeleteMessageAsync(int id)
    {
        var currentUsername = _httpContext.User.GetUsername() ??
                              throw new InvalidOperationException(SD.InvalidOperationMessage);

        var message = await _messageRepository.GetByIdAsync(id) ?? 
                      throw new HttpException(HttpStatusCode.NotFound);

        if (message.SenderUsername != currentUsername && message.RecipientUsername != currentUsername)
            throw new HttpException(HttpStatusCode.Unauthorized);

        if (message.SenderUsername == currentUsername) 
            message.SenderDeleted = true;
        else if (message.RecipientUsername == currentUsername)
            message.RecipientDeleted = true;

        if (message is { SenderDeleted: true, RecipientDeleted: true })
            await _messageRepository.Delete(message);

        await _messageRepository.SaveChangesAsync();
    }
}