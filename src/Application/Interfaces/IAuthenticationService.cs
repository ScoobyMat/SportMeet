using Application.Dtos.Authentication;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> Register(RegisterDto registerDto);
        Task<AuthResponseDto> Login(LoginDto loginDto);
    }
}
