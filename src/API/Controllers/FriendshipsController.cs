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

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<FriendshipDto>>> GetFriends(int userId)
        {
            var list = await _friendshipService.GetFriendsAsync(userId);
            return Ok(list);
        }
    }
}
