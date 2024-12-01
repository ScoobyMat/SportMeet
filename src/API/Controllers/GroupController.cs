using Application.Dtos.GroupDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class GroupController : BaseApiController
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroups()
        {
            var groupDtos = await _groupService.GetAllGroupsAsync();

            if (groupDtos == null || !groupDtos.Any())
            {
                return NoContent();
            }

            return Ok(groupDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroup(int id)
        {
            var groupDto = await _groupService.GetGroupByIdAsync(id);
            if (groupDto == null)
                return NotFound();

            return Ok(groupDto);
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetGroupByEventId(int eventId)
        {
            var groupDto = await _groupService.GetGroupByEventIdAsync(eventId);
            if (groupDto == null)
                return NotFound();

            return Ok(groupDto);
        }

    }
}
