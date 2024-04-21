using DatingApp.BL.DTO.MessagesDTOs;
using DatingApp.BL.Infrastructure;
using DatingApp.BL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
        private readonly IMessageService _messageService;

        public MessagesController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> AddMessageAsync(CreateMessageDto createMessageDto)
        {
            var messageDto = await _messageService.AddMessageAsync(createMessageDto);

            return messageDto;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUserAsync([FromQuery] MessageParams messageParams)
        {
            var messagesDto = await _messageService.GetMessagesForUserAsync(messageParams);

            return Ok(messagesDto);
        }

        [HttpGet("thread/{username}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThreadAsync(string username)
        {
            var messagesDto = await _messageService.GetMessageThreadAsync(username);

            return Ok(messagesDto);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMessageAsync(int id)
        {
            await _messageService.DeleteMessageAsync(id);

            return Ok();
        }
    }
}
