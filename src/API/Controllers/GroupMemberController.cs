using Application.Dtos.GroupDtos;
using Application.Dtos.GroupMemberDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    public class GroupMemberController : BaseApiController
    {
        private readonly IGroupMemberService _groupMemberService;

        public GroupMemberController(IGroupMemberService groupMemberService)
        {
            _groupMemberService = groupMemberService;
        }

        [HttpPost("AddMember")]
        public async Task<IActionResult> AddMember(AddMemberDto addMemberDto)
        {
            try
            {
                await _groupMemberService.AddMemberAsync(addMemberDto);
                return Ok("Member added successfully.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("UpdateManager")]
        public async Task<IActionResult> UpdateManager(GroupUpdateDto groupUpdateDto)
        {
            try
            {
                await _groupMemberService.UpdateManagerAsync(groupUpdateDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("RemoveMember")]
        public async Task<IActionResult> RemoveMember(RemoveMemberDto removeMemberDto)
        {
            try
            {
                await _groupMemberService.RemoveMemberAsync(removeMemberDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
