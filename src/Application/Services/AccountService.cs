using Application.Dtos.Auth;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using System.Globalization;

namespace Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountService(IUserRepository userRepository, ITokenService tokenService, IMapper mapper)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new UnauthorizedAccessException("Nieprawidłowy email");
            }

            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Nieprawidłowe hasło");
            }

            return new AuthResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhotoUrl = user.ProfilePhoto?.Url,
                Token = _tokenService.CreateToken(user)
            };
        }


        public async Task<AuthResponseDto> Register(RegisterDto registerDto)
        {
            if (await _userRepository.ExistsByEmailAsync(registerDto.Email))
            {
                throw new ArgumentException("Istnieje już użytkownik o takim adresie email");
            }

            if (!DateOnly.TryParseExact(registerDto.DateOfBirth, "yyyy-MM-dd",
                 CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate))
            {
                throw new FormatException("Podaj datę urodzenia w formacie yyyy-MM-dd.");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

            var user = _mapper.Map<User>(registerDto);
            user.Password = hashedPassword;

            await _userRepository.AddAsync(user);

            return new AuthResponseDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = _tokenService.CreateToken(user)

        };
        }

    }
}
