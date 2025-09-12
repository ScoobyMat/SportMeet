using Application.Dtos.FriendshipDtos;

namespace Application.Interfaces
{
    public interface IFriendshipService
    {
        Task<FriendshipDto> SendFriendRequestAsync(FriendshipCreateDto dto);
        Task<FriendshipDto> RespondToRequestAsync(FriendshipRespondDto dto);
        Task DeleteFriendshipAsync(int friendshipId);
        Task<List<FriendshipDto>> GetFriendsAsync(int userId);
        Task<List<FriendshipDto>> GetSentInvitationsAsync(int userId);
        Task<List<FriendshipDto>> GetReceivedInvitationsAsync(int userId);
    }
}
