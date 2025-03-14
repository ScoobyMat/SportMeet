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

        /// <summary>
        /// Pobieranie wszystkich wiadomości dla danego eventu
        /// </summary>
        [HttpGet("{eventId}")]
        public async Task<ActionResult<IEnumerable<EventMessageDto>>> GetMessagesForEvent(int eventId)
        {
            var messages = await _service.GetMessagesForEventAsync(eventId);
            return Ok(messages);
        }

        /// <summary>
        /// Utworzenie wiadomości 
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<EventMessageDto>> CreateMessage(EventMessageCreateDto dto)
        {
            var result = await _service.SendMessageAsync(dto);
            return Ok(result);
        }

        /// <summary>
        /// Usunięcie danej wiadomości 
        /// </summary>
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
