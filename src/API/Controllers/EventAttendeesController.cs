using Application.Dtos.EventAttendeeDtos;
using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventAttendeesController : ControllerBase
    {
        private readonly IEventAttendeeService _attendeeService;

        public EventAttendeesController(IEventAttendeeService attendeeService)
        {
            _attendeeService = attendeeService;
        }


        /// <summary>
        /// Pobranie listy wszystkich uczestników danego wydarzenia.
        /// </summary>
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<EventAttendeeDto>>> GetAttendeesForEvent(int eventId)
        {
            var attendees = await _attendeeService.GetByEventIdAsync(eventId);
            return Ok(attendees);
        }

        /// <summary>
        /// Zapisanie użytkownika na konkretne wydarzenie.
        /// </summary>
        [HttpPost("event/{eventId}/user/{userId}")]
        public async Task<ActionResult<EventAttendeeDto>> JoinEvent([FromRoute] int eventId, [FromRoute] int userId)
        {
            try
            {
                var attendee = await _attendeeService.AddAttendeeAsync(eventId, userId);
                return Ok(attendee);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Wypisanie użytkownika z danego wydarzenia.
        /// </summary>
        [HttpDelete("event/{eventId}/user/{userId}")]
        public async Task<IActionResult> LeaveEvent(int eventId, int userId)
        {
            var attendee = await _attendeeService.GetByEventAndUserAsync(eventId, userId);
            if (attendee == null)
            {
                return NotFound("Attendee not found for this event.");
            }
            await _attendeeService.DeleteAttendeeAsync(attendee.Id);
            return NoContent();
        }
    }
}
