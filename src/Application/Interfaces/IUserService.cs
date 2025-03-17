using Application.Dtos.UserDtos;

namespace Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<UserDto?> GetUserByUsernameAsync(string username);
    Task<UserDto> UpdateUserAsync(UserUpdateDto updateDto);
    Task<UserDto> ChangeCredentialsAsync(ChangeCredentialsDto dto);
    Task DeleteUserAsync(int id);

}
