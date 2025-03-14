using Application.Dtos.FriendshipDtos;
using Application.Dtos.NotificationDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{

    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly INotificationService _notificationService;

        public FriendshipService(IFriendshipRepository friendshipRepo,
                                 IUserRepository userRepo,
                                 IMapper mapper,
                                 INotificationService notificationService)
        {
            _friendshipRepo = friendshipRepo;
            _userRepo = userRepo;
            _mapper = mapper;
            _notificationService = notificationService;
        }

        public async Task<FriendshipDto> SendFriendRequestAsync(FriendshipCreateDto dto)
        {
            var requestor = await _userRepo.GetByIdAsync(dto.RequestorId);
            if (requestor == null)
                throw new KeyNotFoundException("Requestor not found.");

            var addressee = await _userRepo.GetByIdAsync(dto.AddresseeId);
            if (addressee == null)
                throw new KeyNotFoundException("Addressee not found.");

            var existing = await _friendshipRepo.GetRelationAsync(dto.RequestorId, dto.AddresseeId);
            if (existing != null)
            {
                if (existing.Status == FriendshipStatus.Rejected)
                {
                    existing.Status = FriendshipStatus.Pending;
                    existing.Created = DateTime.UtcNow;
                    await _friendshipRepo.UpdateAsync(existing);


                    var loadedAgain = await _friendshipRepo.GetByIdAsync(existing.Id);
                    return _mapper.Map<FriendshipDto>(loadedAgain);
                }
                else
                {
                    throw new InvalidOperationException("Relation already exists in a different status.");
                }
            }

            var friendship = _mapper.Map<Friendship>(dto);
            friendship.Created = DateTime.UtcNow;
            friendship.Status = FriendshipStatus.Pending;

            await _friendshipRepo.AddAsync(friendship);

            var notificationCreateDto = new NotificationCreateDto
            {
                UserId = addressee.Id,
                Type = "FriendRequest",
                Message = $"Masz nowe zaproszenie do znajomych od {requestor.FirstName} {requestor.LastName}"
            };
            await _notificationService.CreateAsync(notificationCreateDto);

            var loaded = await _friendshipRepo.GetByIdAsync(friendship.Id);
            return _mapper.Map<FriendshipDto>(loaded);
        }

        public async Task<FriendshipDto> RespondToRequestAsync(FriendshipRespondDto dto)
        {
            var friendship = await _friendshipRepo.GetByIdAsync(dto.FriendshipId);
            if (friendship == null)
                throw new KeyNotFoundException("Friendship not found");

            if (friendship.Status != FriendshipStatus.Pending)
                throw new InvalidOperationException("Friendship is not in pending state.");

            friendship.Status = dto.Accept ? FriendshipStatus.Accepted : FriendshipStatus.Rejected;
            await _friendshipRepo.UpdateAsync(friendship);

            if (dto.Accept)
            {
                var addressee = await _userRepo.GetByIdAsync(friendship.AddresseeId);
                if (addressee != null)
                {
                    var notificationCreateDto = new NotificationCreateDto
                    {
                        UserId = friendship.RequestorId,
                        Type = "FriendRequestAccepted",
                        Message = $"Twoje zaproszenie do znajomych zostało zaakceptowane przez {addressee.FirstName} {addressee.LastName}"
                    };
                    await _notificationService.CreateAsync(notificationCreateDto);
                }
            }

            var loaded = await _friendshipRepo.GetByIdAsync(friendship.Id);
            return _mapper.Map<FriendshipDto>(loaded);
        }

        public async Task DeleteFriendshipAsync(int friendshipId)
        {
            var friendship = await _friendshipRepo.GetByIdAsync(friendshipId);
            if (friendship == null)
                throw new KeyNotFoundException("Friendship not found");

            await _friendshipRepo.DeleteAsync(friendship);
        }

        public async Task<List<FriendshipDto>> GetFriendsAsync(int userId)
        {
            var list = await _friendshipRepo.GetAcceptedFriendshipsForUserAsync(userId);
            return list.Select(f => _mapper.Map<FriendshipDto>(f)).ToList();
        }

        public async Task<List<FriendshipDto>> GetReceivedInvitationsAsync(int userId)
        {
            var invitations = await _friendshipRepo.GetReceivedInvitationsAsync(userId);
            return _mapper.Map<List<FriendshipDto>>(invitations);
        }
    }
}
