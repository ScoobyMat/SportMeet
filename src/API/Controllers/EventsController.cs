using Application.Dtos.EventDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class EventsController : BaseApiController
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EventDto>> GetById(int id)
        {
            try
            {
                var ev = await _eventService.GetByIdAsync(id);
                return Ok(ev);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<EventDto>> Create(EventCreateDto dto)
        {
            try
            {
                var created = await _eventService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<EventDto>> Update(EventUpdateDto dto)
        {
            try
            {
                var updated = await _eventService.UpdateAsync(dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _eventService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("{eventId}/join/{userId}")]
        public async Task<ActionResult> JoinEvent(int eventId, int userId)
        {
            try
            {
                await _eventService.JoinEventAsync(eventId, userId);
                return Ok();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("user/{userId}/upcoming")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetUpcomingEventsForUser(int userId)
        {
            var events = await _eventService.GetUpcomingEventsForUserAsync(userId);
            return Ok(events);
        }

        [HttpGet("user/{userId}/managed")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetManagedEvents(int userId)
        {
            var events = await _eventService.GetManagedEventsAsync(userId);
            return Ok(events);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EventDto>>> SearchEvents([FromQuery] EventSearchDto searchDto)
        {
            var events = await _eventService.SearchEventsAsync(searchDto);
            return Ok(events);
        }
    }
}
