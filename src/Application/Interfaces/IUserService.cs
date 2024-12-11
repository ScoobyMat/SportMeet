using Application.Dtos.UserDtos;

namespace Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> GetUserByEmailAsync(string email);
    Task UpdateUserAsync(UserUpdateDto updateDto);
    Task DeleteUserAsync(int id);

}
