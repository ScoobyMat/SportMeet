using Application.Dtos.NotificationDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class NotificationsController : BaseApiController
    {
        private readonly INotificationService _service;

        public NotificationsController(INotificationService service)
        {
            _service = service;
        }

        /// <summary>
        /// Pobranie wszystkich powiadomień użytkownika
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetAllForUser(int userId)
        {
            var notifs = await _service.GetAllByUserAsync(userId);
            return Ok(notifs);
        }

        /// <summary>
        /// Pobranie tylko nieprzeczytanych powiadomienie danego użytkownika
        /// </summary>
        [HttpGet("user/{userId}/unread")]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetUnreadForUser(int userId)
        {
            var notifs = await _service.GetUnreadByUserAsync(userId);
            return Ok(notifs);
        }


       /* /// <summary>
        /// Tworzenie powiadomienia
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<NotificationDto>> Create(NotificationCreateDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return Ok(result);
        }*/

        /// <summary>
        /// Oznaczenie pojedyńczego powiadomienia jako przeczytane
        /// </summary>
        [HttpPatch("{id}/read")]
        public async Task<ActionResult> MarkAsRead(int id)
        {
            try
            {
                await _service.MarkAsReadAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Oznaczenie wszystkich powiadomień użytkownika jako przeczytane
        /// </summary>
        [HttpPatch("user/{userId}/readall")]
        public async Task<ActionResult> MarkAllAsRead(int userId)
        {
            try
            {
                await _service.MarkAllAsReadAsync(userId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /*/// <summary>
        /// Usunienie danego powiadomienia
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNotification(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }*/
    }
}
