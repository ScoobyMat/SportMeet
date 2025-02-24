using Application.Dtos.PrivateMessageDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PrivateMessagesController : BaseApiController
    {
        private readonly IPrivateMessageService _service;

        public PrivateMessagesController(IPrivateMessageService service)
        {
            _service = service;
        }

        [HttpGet("conversation")]
        public async Task<ActionResult<IEnumerable<PrivateMessageDto>>> GetConversation(
            [FromQuery] int userAId, [FromQuery] int userBId)
        {
            var messages = await _service.GetConversationAsync(userAId, userBId);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<ActionResult<PrivateMessageDto>> SendMessage(PrivateMessageCreateDto dto)
        {
            var result = await _service.SendMessageAsync(dto);
            return Ok(result);
        }


        [HttpDelete("{messageId}")]
        public async Task<ActionResult> DeleteMessage(int messageId)
        {
            try
            {
                await _service.DeleteMessageAsync(messageId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
