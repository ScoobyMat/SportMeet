using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class GroupController(IGroupService groupService) : BaseApiController
    {
        [HttpPost("{groupId}/addMemberByUserId")]
        public async Task<ActionResult> AddMemberByUserId(int groupId, int userId)
        {
            var success = await groupService.AddMemberToGroupByUserId(groupId, userId);
            if (!success)
            {
                return BadRequest("Failed to add member.");
            }

            return Ok("Member added successfully.");
        }

        [HttpPost("{groupId}/addMemberByEmail")]
        public async Task<ActionResult> AddMemberByEmail(int groupId, string email)
        {
            var success = await groupService.AddMemberToGroupByEmail(groupId, email);
            if (!success)
            {
                return BadRequest("Failed to add member.");
            }

            return Ok("Member added successfully.");
        }

        [HttpDelete("{groupId}/removeMemberByUserId")]
        public async Task<ActionResult> RemoveMemberByUserId(int groupId, int userId)
        {
            if (userId <= 0)
            {
                return BadRequest("Invalid user ID.");
            }

            var success = await groupService.RemoveMemberFromGroup(groupId, userId);
            if (!success)
            {
                return BadRequest("Failed to remove member.");
            }

            return Ok("Member removed successfully.");
        }

    }
}
