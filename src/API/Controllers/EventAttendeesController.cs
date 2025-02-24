using Application.Dtos.EventAttendeeDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EventAttendeesController : BaseApiController
    {
        private readonly IEventAttendeeService _service;

        public EventAttendeesController(IEventAttendeeService service)
        {
            _service = service;
        }

        [HttpGet("{eventId}")]
        public async Task<ActionResult<IEnumerable<EventAttendeeDto>>> GetAttendeesForEvent(int eventId)
        {
            var attendees = await _service.GetByEventIdAsync(eventId);
            return Ok(attendees);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<EventAttendeeDto>> GetAttendee(int id)
        {
            var attendee = await _service.GetByIdAsync(id);
            if (attendee == null) return NotFound("Attendee not found");
            return Ok(attendee);
        }

        [HttpPost]
        public async Task<ActionResult<EventAttendeeDto>> AddAttendee(EventAttendeeCreateDto dto)
        {
            var result = await _service.AddAttendeeAsync(dto);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAttendee(EventAttendeeUpdateDto dto)
        {
            try
            {
                await _service.UpdateAttendeeAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAttendee(int id)
        {
            try
            {
                await _service.DeleteAttendeeAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
