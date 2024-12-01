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
                throw new ArgumentException("Group not found");

            var user = await _userRepository.GetByIdAsync(addMemberDto.UserId);
            if (user == null)
                throw new ArgumentException("User not found");

            var existingMember = group.Members.Find(m => m.UserId == user.Id);
            if (existingMember != null)
                throw new ArgumentException("User is already a member of this group");

            var groupMember = new GroupMember
            {
                GroupId = addMemberDto.GroupId,
                UserId = addMemberDto.UserId,
                Role = addMemberDto.Role
            };

            await _groupMemberRepository.AddMemberAsync(groupMember);
        }

        public async Task<bool> RemoveMemberAsync(RemoveMemberDto removeMemberDto)
        {
            var group = await _groupRepository.GetByIdAsync(removeMemberDto.GroupId);
            if (group == null)
                return false;

            var member = group.Members.FirstOrDefault(m => m.UserId == removeMemberDto.UserId);
            if (member == null)
                return false;

            await _groupMemberRepository.RemoveMemberAsync(removeMemberDto.GroupId, removeMemberDto.UserId);
            return true;
        }

        public async Task<bool> UpdateManagerAsync(GroupUpdateDto groupUpdateDto)
        {
            var group = await _groupRepository.GetByIdAsync(groupUpdateDto.GroupId);
            if (group == null)
                return false;

            var currentManager = group.Members.FirstOrDefault(m => m.UserId == groupUpdateDto.CurrentManagerId);
            if (currentManager == null)
                return false;

            var newManager = group.Members.FirstOrDefault(m => m.UserId == groupUpdateDto.NewManagerId);
            if (newManager == null)
                return false;

            currentManager.Role = "Member";
            newManager.Role = "Manager";

            await _groupMemberRepository.UpdateMemberAsync(currentManager);
            await _groupMemberRepository.UpdateMemberAsync(newManager);

            return true;
        }
    }
}
