using Application.Dtos;
using Application.Dtos.EventDtos;
using Application.Dtos.UserDtos;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EventController(IEventService eventService) : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {
            var events = await eventService.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            var eventDto = await eventService.GetEventByIdAsync(id);

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
                var eventResponse = await eventService.AddEventAsync(eventDto);

                return CreatedAtAction(nameof(GetEvent), new { id = eventResponse?.Id }, eventResponse);
            }
            catch (Exception ex)
            {
                return NotFound(new { Message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(EventUpdateDto eventUpdate)
        {
            var success = await eventService.UpdateEventAsync(eventUpdate);
            if (!success)
            {
                return NotFound("Event not found.");
            }

            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {

            var success = await eventService.DeleteEventAsync(id);
            if (!success)
            {
                return NotFound("Event not found.");
            }

            return NoContent();

        }
    }
}
