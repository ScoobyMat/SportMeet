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

        /// <summary>
        /// Pobranie wszystkich wydarzeń
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        /// <summary>
        /// Pobieranie konkretnego wydarzenia
        /// </summary>
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

        /// <summary>
        /// Utworzenie wydarzenia
        /// </summary>
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

        /// <summary>
        /// Aktualizacja danych wydarzenia
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<EventDto>> Update([FromForm] EventUpdateDto dto)
        {
            try
            {
                var updated = await _eventService.UpdateAsync(dto);
                return Ok(updated);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        /// <summary>
        /// Usunięcie danego wydarzenia
        /// </summary>
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
        /// <summary>
        /// Pobranie nadchodzących wydarzeń
        /// </summary>
        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetUpcomingEvents()
        {
            var events = await _eventService.GetUpcomingEventsAsync();
            return Ok(events);
        }

        /// <summary>
        /// Pobranie minionych wydarzeń
        /// </summary>
        [HttpGet("past")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetPastEvents()
        {
            var events = await _eventService.GetPastEventsAsync();
            return Ok(events);
        }

        /// <summary>
        /// Pobranie nadchodzących wydarzeń danego użytkownika
        /// </summary>
        [HttpGet("user/{userId}/upcoming")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetUpcomingEventsForUser(int userId)
        {
            var events = await _eventService.GetUpcomingEventsForUserAsync(userId);
            return Ok(events);
        }

        /// <summary>
        /// Pobranie mininych wydarzeń danego użytkownika
        /// </summary>
        [HttpGet("user/{userId}/past")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetPastEventsForUser(int userId)
        {
            var events = await _eventService.GetPastEventsForUserAsync(userId);
            return Ok(events);
        }

        /// <summary>
        /// Pobranie organizowanych wydarzeń przez danego użytkownika
        /// </summary>
        [HttpGet("user/{userId}/managed")]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetManagedEvents(int userId)
        {
            var events = await _eventService.GetManagedEventsAsync(userId);
            return Ok(events);
        }

        /// <summary>
        /// Pobranie wydzarzeń spełniających kryteria wyszukiwania
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EventDto>>> SearchEvents([FromQuery] EventSearchDto searchDto)
        {
            var events = await _eventService.SearchEventsAsync(searchDto);
            return Ok(events);
        }
    }
}
