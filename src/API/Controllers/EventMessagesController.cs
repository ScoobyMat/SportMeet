using Application.Dtos.EventMessageDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EventMessagesController : BaseApiController
    {
        private readonly IEventMessageService _service;

        public EventMessagesController(IEventMessageService service)
        {
            _service = service;
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult<IEnumerable<EventMessageDto>>> GetMessagesForEvent(int eventId)
        {
            var messages = await _service.GetMessagesForEventAsync(eventId);
            return Ok(messages);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<EventMessageDto>> GetMessage(int id)
        {
            try
            {
                var msg = await _service.GetMessageByIdAsync(id);
                return Ok(msg);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<EventMessageDto>> CreateMessage(EventMessageCreateDto dto)
        {
            var result = await _service.CreateMessageAsync(dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMessage(int id)
        {
            try
            {
                await _service.DeleteMessageAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
