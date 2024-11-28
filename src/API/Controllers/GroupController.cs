using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class GroupController : BaseApiController
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpPost("addMemberByUserId")]
        public async Task<ActionResult> AddMemberByUserId(int groupId, int userId)
        {
            try
            {
                await _groupService.AddMemberToGroupByUserIdAsync(groupId, userId);
                return Ok("Member added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("addMemberByEmail")]
        public async Task<ActionResult> AddMemberByEmail(int groupId, string email)
        {
            try
            {
                await _groupService.AddMemberToGroupByEmailAsync(groupId, email);
                return Ok("Member added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("removeMember")]
        public async Task<ActionResult> RemoveMemberByUserId(int groupId, int userId)
        {
            try
            {
                await _groupService.RemoveMemberFromGroupAsync(groupId, userId);
                return Ok("Member removed successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
