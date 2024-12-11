using Application.Dtos.GroupDtos;
using Application.Dtos.GroupMemberDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupMemberService _groupMemberService;
        private readonly IMapper _mapper;

        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository, IGroupMemberService groupMemberService, IMapper mapper)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _groupMemberService = groupMemberService;
            _mapper = mapper;
        }

        public async Task<GroupDto> CreateGroupAsync(GroupCreateDto groupCreateDto)
        {
            var user = await _userRepository.GetByIdAsync(groupCreateDto.CreatedByUserId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User not found.");
            }

            var group = _mapper.Map<Group>(groupCreateDto);
            await _groupRepository.AddAsync(group);

            var addMemberDto = new AddMemberDto
            {
                GroupId = group.Id,
                UserId = groupCreateDto.CreatedByUserId,
                Role = "Manager"
            };

            await _groupMemberService.AddMemberAsync(addMemberDto);

            return _mapper.Map<GroupDto>(group);
        }

        public async Task<GroupDto?> GetGroupByEventIdAsync(int eventId)
        {
            var group = await _groupRepository.GetGroupByEventIdAsync(eventId);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group not found.");
            }

            return _mapper.Map<GroupDto>(group);
        }

        public async Task<GroupDto?> GetGroupByIdAsync(int groupId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group not found.");
            }

            return _mapper.Map<GroupDto>(group);
        }

        public async Task<IEnumerable<GroupDto>> GetAllGroupsAsync()
        {
            var groups = await _groupRepository.GetAllAsync();

            if (groups == null || !groups.Any())
            {
                throw new InvalidOperationException("No groups found.");
            }

            return _mapper.Map<IEnumerable<GroupDto>>(groups);
        }
    }
}
