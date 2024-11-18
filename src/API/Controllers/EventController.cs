using Application.DTOs;
using Application.DTOs.Event;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EventController(IEventService eventService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {
            var events = await eventService.GetEvents();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var eventDto = await eventService.GetEvent(id);

            if (eventDto == null)
            {
                return NotFound();
            }

            return Ok(eventDto);
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> AddEvent(EventCreateDto eventDto)
        {
            try
            {
                var eventResponse = await eventService.AddEvent(eventDto);

                return CreatedAtAction(nameof(GetEvent), new { id = eventResponse?.Id }, eventResponse);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateEvent(EventUpdateDto updatedEvent)
        {
            if (updatedEvent == null)
            {
                return BadRequest();
            }

            var success =  await eventService.UpdateEvent(updatedEvent);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            var success = await eventService.DeleteEvent(id);

            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
