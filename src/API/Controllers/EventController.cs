using Application.Dtos.EventDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // [Authorize]
    public class EventController : BaseApiController
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAllEvents()
        {
            try
            {
                var events = await _eventService.GetAllEventsAsync();
                return Ok(events);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetEvent(int id)
        {
            try
            {
                var eventDto = await _eventService.GetEventByIdAsync(id);
                return Ok(eventDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredEvents([FromQuery] string? location, [FromQuery] DateOnly? startDate, [FromQuery] DateOnly? endDate)
        {
            try
            {
                var events = await _eventService.GetFilteredEventsAsync(location, startDate, endDate);
                return Ok(events);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> AddEvent([FromBody] EventCreateDto eventDto)
        {
            try
            {
                var result = await _eventService.AddEventAsync(eventDto);
                return CreatedAtAction(nameof(GetEvent), new { id = result?.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("upcoming-events/{userId}")]
        public async Task<IActionResult> GetUpcomingEventsForUser(int userId)
        {
            try
            {
                var events = await _eventService.GetUpcomingEventsForUserAsync(userId);
                return Ok(events);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEvent(int id, [FromBody] EventUpdateDto eventUpdate)
        {
            try
            {
                eventUpdate.Id = id;
                await _eventService.UpdateEventAsync(eventUpdate);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEvent(int id)
        {
            try
            {
                await _eventService.DeleteEventAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
