using Application.Dtos.GroupDtos;
using Application.Dtos.GroupMemberDtos;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class GroupMemberController(IGroupMemberService groupMemberService) : BaseApiController
    {
        [HttpPost("AddMember")]
        public async Task<IActionResult> AddMember(AddMemberDto addMemberDto)
        {
            try
            {
                await groupMemberService.AddMemberAsync(addMemberDto);
                return Ok("Member added successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateMember")]
        public async Task<IActionResult> UpdateManager(GroupUpdateDto groupUpdateDto)
        {
            var success = await groupMemberService.UpdateManagerAsync(groupUpdateDto);
            if (!success)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("RemoveMembere")]
        public async Task<IActionResult> RemoveMember(RemoveMemberDto removeMemberDto)
        {
            var success = await groupMemberService.RemoveMemberAsync(removeMemberDto);
            if (!success)
                return NotFound();

            return NoContent();

            /*try
            {
                await _groupService.RemoveMemberFromGroupAsync(groupId, userId);
                return Ok("Member removed successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }*/
        }
        

    }
}
