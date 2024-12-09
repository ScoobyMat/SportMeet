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

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredEvents([FromQuery] string? location, [FromQuery] DateOnly? startDate, [FromQuery] DateOnly? endDate)
        {
            var events = await eventService.GetFilteredEventsAsync(location, startDate, endDate);
            return Ok(events);
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> AddEvent([FromBody] EventCreateDto eventDto)
        {
            var result = await eventService.AddEventAsync(eventDto);
            if (result == null)
            {
                return BadRequest("Event creation failed.");
            }

            return CreatedAtAction(nameof(GetEvent), new { id = result.Id }, result);
        }

        [HttpGet("upcoming-events/{userId}")]
        public async Task<IActionResult> GetUpcomingEventsForUser(int userId)
        {
            var events = await eventService.GetUpcomingEventsForUserAsync(userId);

            if (events == null || !events.Any())
            {
                return NotFound("No upcoming events found for this user.");
            }

            return Ok(events);
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
