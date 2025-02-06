using Application.Dtos.Authentication;
using Application.Dtos.UserDtos;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<UserDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
    }
}
