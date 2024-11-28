using Application.Interfaces;
using Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;

        public GroupService(IGroupRepository groupRepository, IUserRepository userRepository)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }

        public async Task AddMemberToGroupByEmailAsync(int groupId, string email)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null)
                throw new ArgumentException("Group not found");

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                throw new ArgumentException("User not found");

            var existingMember = group.Members.Find(m => m.UserId == user.Id);
            if (existingMember != null)
                throw new ArgumentException("User is already a member of this group");

            await _groupRepository.AddMemberToGroupAsync(groupId, user.Id);
        }

        public async Task AddMemberToGroupByUserIdAsync(int groupId, int userId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null)
                throw new ArgumentException("Group not found");

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var existingMember = group.Members.Find(m => m.UserId == user.Id);
            if (existingMember != null)
                throw new ArgumentException("User is already a member of this group");

            await _groupRepository.AddMemberToGroupAsync(groupId, userId);
        }

        public async Task RemoveMemberFromGroupAsync(int groupId, int userId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null)
                throw new ArgumentException("Group not found");

            var existingMember = group.Members.Find(m => m.UserId == userId);
            if (existingMember == null)
                throw new ArgumentException("User is not a member of this group");

            await _groupRepository.RemoveMemberFromGroupAsync(groupId, userId);
        }
    }
}
