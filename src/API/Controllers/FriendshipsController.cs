using Application.Dtos.FriendshipDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FriendshipsController : BaseApiController
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipsController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }
        /// <summary>
        /// Wysłanie zaproszenia do znajomych
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FriendshipDto>> SendFriendRequest(FriendshipCreateDto dto)
        {
            try
            {
                var result = await _friendshipService.SendFriendRequestAsync(dto);
                return Created(nameof(SendFriendRequest), result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Odpowiedz na zaproszenie do znajomych
        /// </summary>
        [HttpPost("respond")]
        public async Task<ActionResult<FriendshipDto>> Respond(FriendshipRespondDto dto)
        {
            try
            {
                var result = await _friendshipService.RespondToRequestAsync(dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Usunięcie zaproszenia
        /// </summary>
        [HttpDelete("{friendshipId}")]
        public async Task<ActionResult> Delete(int friendshipId)
        {
            try
            {
                await _friendshipService.DeleteFriendshipAsync(friendshipId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Pobranie znajomych danego użytkownika
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<FriendshipDto>>> GetFriends(int userId)
        {
            var list = await _friendshipService.GetFriendsAsync(userId);
            return Ok(list);
        }

        /// <summary>
        /// Pobranie wysłanych zaproszeń do znajomych przez danego użytkownika
        /// </summary>
        [HttpGet("sent/{userId}")]
        public async Task<ActionResult<List<FriendshipDto>>> GetSentInvitations([FromRoute] int userId)
        {
            try
            {
                var invitations = await _friendshipService.GetSentInvitationsAsync(userId);
                return Ok(invitations);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Pobranie oczekujacych zaproszeń do znajomych dla danego użytkownika
        /// </summary>
        [HttpGet("received/{userId}")]
        public async Task<ActionResult<List<FriendshipDto>>> GetReceivedInvitations(int userId)
        {
            try
            {
                var invitations = await _friendshipService.GetReceivedInvitationsAsync(userId);
                return Ok(invitations);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
