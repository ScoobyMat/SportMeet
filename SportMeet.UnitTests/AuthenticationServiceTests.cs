using Application.Dtos.Authentication;
using Application.Dtos.UserDtos;
using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;

namespace SportMeet.UnitTests.Services
{
    public class AuthenticationServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<ITokenService> _tokenServiceMock;
        private readonly IMapper _mapper;
        private readonly AuthenticationService _authService;

        public AuthenticationServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _tokenServiceMock = new Mock<ITokenService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterDto, User>()
                    .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => DateOnly.Parse(src.DateOfBirth)));
                cfg.CreateMap<User, UserDto>();
            });
            _mapper = config.CreateMapper();

            _authService = new AuthenticationService(
                _userRepositoryMock.Object,
                _tokenServiceMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task Register_WithValidData_ReturnsUserDto()
        {
            var registerDto = new RegisterDto
            {
                FirstName = "Test",
                LastName = "User",
                Gender = "Mężczyzna",
                DateOfBirth = "2000-01-01",
                City = "Warszawa",
                Country = "Polska",
                Email = "testuser@example.com",
                Password = "Password123"
            };

            // Email nie istnieje, więc rejestracja powinna przebiegać poprawnie
            _userRepositoryMock.Setup(repo => repo.ExistsByEmailAsync(registerDto.Email))
                .ReturnsAsync(false);

            var user = new User
            {
                Id = 1,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Gender = registerDto.Gender,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                DateOfBirth = DateOnly.Parse(registerDto.DateOfBirth),
                City = registerDto.City,
                Country = registerDto.Country
            };

            _userRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<User>()))
                .Returns(Task.CompletedTask)
                .Callback<User>(u => u.Id = user.Id);

            var result = await _authService.Register(registerDto);

            Assert.NotNull(result);
            Assert.Equal("Test", result.FirstName);
            Assert.Equal("User", result.LastName);
            _userRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Register_WithExistingEmail_ThrowsArgumentException()
        {
            var registerDto = new RegisterDto
            {
                FirstName = "Test",
                LastName = "User",
                Gender = "Mężczyzna",
                DateOfBirth = "2000-01-01",
                City = "Warszawa",
                Country = "Polska",
                Email = "testuser@example.com",
                Password = "Password123"
            };

            // Email istnieje, więc rejestracja nie powinna się ukończy (więc exception)
            _userRepositoryMock.Setup(repo => repo.ExistsByEmailAsync(registerDto.Email))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<ArgumentException>(() => _authService.Register(registerDto));
        }
    }
}
