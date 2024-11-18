using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Linq;
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

        public async Task<bool> AddMemberToGroupByUserId(int groupId, int userId)
        {
            var group = await _groupRepository.GetGroup(groupId);
            if (group == null)
            {
                return false;
            }

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return false;
            }

            if (group.Members.Any(m => m.Id == userId))
            {
                return false;
            }

            group.Members.Add(user);
            _groupRepository.UpdateGroup(group);

            return true;
        }

        public async Task<bool> AddMemberToGroupByEmail(int groupId, string email)
        {
            var group = await _groupRepository.GetGroup(groupId);
            if (group == null)
            {
                return false;
            }

            var user = await _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return false;
            }

            if (group.Members.Any(m => m.Id == user.Id))
            {
                return false;
            }

            group.Members.Add(user);
            _groupRepository.UpdateGroup(group);
            return true;
        }

        public async Task<bool> RemoveMemberFromGroup(int groupId, int userId)
        {
            // Pobranie grupy
            var group = await _groupRepository.GetGroup(groupId);
            if (group == null)
            {
                return false; // Grupa nie istnieje
            }

            // Znalezienie użytkownika w grupie
            var user = group.Members.FirstOrDefault(m => m.Id == userId);
            if (user == null)
            {
                return false; // Użytkownik nie jest członkiem grupy
            }

            // Usuwanie użytkownika z grupy
            group.Members.Remove(user);

            // Zaktualizowanie grupy w repozytorium i zapisanie zmian
            _groupRepository.UpdateGroup(group);

            return true; // Użytkownik został usunięty
        }
    }
}
