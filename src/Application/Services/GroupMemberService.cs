using Application.Dtos.GroupDtos;
using Application.Dtos.GroupMemberDtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class GroupMemberService : IGroupMemberService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IUserRepository _userRepository;

        public GroupMemberService(IGroupRepository groupRepository, IGroupMemberRepository groupMemberRepository, IUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _groupMemberRepository = groupMemberRepository;
            _userRepository = userRepository;
        }

        public async Task AddMemberAsync(AddMemberDto addMemberDto)
        {
            var group = await _groupRepository.GetByIdAsync(addMemberDto.GroupId);
            if (group == null)
                throw new KeyNotFoundException("Group not found.");

            var user = await _userRepository.GetByIdAsync(addMemberDto.UserId);
            if (user == null)
                throw new KeyNotFoundException("User not found.");

            var existingMember = group.Members.Find(m => m.UserId == user.Id);
            if (existingMember != null)
                throw new InvalidOperationException("User is already a member of the group.");

            if (group.Members.Count >= group.MaxMembers)
            {
                throw new InvalidOperationException("Group is already full.");
            }

            var groupMember = new GroupMember
            {
                GroupId = addMemberDto.GroupId,
                UserId = addMemberDto.UserId,
                Role = addMemberDto.Role
            };

            await _groupMemberRepository.AddMemberAsync(groupMember);
        }

        public async Task RemoveMemberAsync(RemoveMemberDto removeMemberDto)
        {
            var group = await _groupRepository.GetByIdAsync(removeMemberDto.GroupId);
            if (group == null)
                throw new KeyNotFoundException("Group not found.");

            var member = group.Members.FirstOrDefault(m => m.UserId == removeMemberDto.UserId);
            if (member == null)
                throw new KeyNotFoundException("User is not a member of the group.");

            await _groupMemberRepository.RemoveMemberAsync(member);
        }

        public async Task UpdateManagerAsync(GroupUpdateDto groupUpdateDto)
        {
            var group = await _groupRepository.GetByIdAsync(groupUpdateDto.GroupId);
            if (group == null)
                throw new KeyNotFoundException($"Group not found.");

            var currentManager = group.Members.FirstOrDefault(m => m.UserId == groupUpdateDto.CurrentManagerId);
            if (currentManager == null)
                throw new KeyNotFoundException($"Current manager is not found in group.");

            var newManager = group.Members.FirstOrDefault(m => m.UserId == groupUpdateDto.NewManagerId);
            if (newManager == null)
                throw new KeyNotFoundException($"New manager is not found in group.");

            currentManager.Role = "Member";
            newManager.Role = "Manager";

            await _groupMemberRepository.UpdateMemberAsync(currentManager);
            await _groupMemberRepository.UpdateMemberAsync(newManager);
        }
    }
}
