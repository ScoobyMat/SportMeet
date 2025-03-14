using Application.Dtos.EventDtos;
using Application.Dtos.EventAttendeeDtos;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Moq;
using Application.Interfaces;
using Xunit;

namespace SportMeet.UnitTests
{
    public class EventServiceTests
    {
        private readonly Mock<IEventRepository> _eventRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IPhotoService> _photoServiceMock;
        private readonly Mock<IGeoCodingService> _geoCodingServiceMock;
        private readonly Mock<IEventAttendeeService> _eventAttendeeServiceMock;
        private readonly IMapper _mapper;
        private readonly EventService _eventService;

        public EventServiceTests()
        {
            _eventRepositoryMock = new Mock<IEventRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _photoServiceMock = new Mock<IPhotoService>();
            _geoCodingServiceMock = new Mock<IGeoCodingService>();
            _eventAttendeeServiceMock = new Mock<IEventAttendeeService>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EventCreateDto, Event>();
                cfg.CreateMap<Event, EventDto>();
            });
            _mapper = config.CreateMapper();

            _eventService = new EventService(
                _eventRepositoryMock.Object,
                _userRepositoryMock.Object,
                _photoServiceMock.Object,
                _geoCodingServiceMock.Object,
                _eventAttendeeServiceMock.Object,
                _mapper
            );
        }

        [Fact]
        public async Task CreateAsync_ValidData_ReturnsEventDto()
        {
            // Arrange
            var eventCreateDto = new EventCreateDto
            {
                EventName = "Test Event",
                Description = "Test Description",
                SportType = "Bieganie",
                MaxParticipants = 10,
                Address = "Test Street 1",
                City = "Warszawa",
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                Time = TimeSpan.FromHours(10),
                CreatedByUserId = 1,
                Photo = null
            };

            var creator = new User
            {
                Id = 1,
                FirstName = "Test",
                LastName = "Test",
                UserName = "Test",
                Email = "Test.Test@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Password123"),
                Gender = "Mê¿czyzna",
                City = "Warszawa",
                Country = "Polska",
                DateOfBirth = new DateOnly(2000, 1, 1)
            };
            _userRepositoryMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(creator);

            _geoCodingServiceMock.Setup(s => s.GetCoordinatesAsync(It.IsAny<Event>()))
                .ReturnsAsync((Event ev) =>
                {
                    ev.Latitude = 52.2297;
                    ev.Longitude = 21.0122;
                    return ev;
                });

            _eventRepositoryMock.Setup(r => r.AddAsync(It.IsAny<Event>()))
                .Returns(Task.CompletedTask)
                .Callback<Event>(ev => ev.Id = 100);

            var attendeeDto = new EventAttendeeDto { Id = 1, EventId = 100, UserId = 1 };
            // Ustawiamy metodê z sygnatur¹ (int, int)
            _eventAttendeeServiceMock.Setup(s => s.AddAttendeeAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(attendeeDto);

            // Act
            var result = await _eventService.CreateAsync(eventCreateDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Event", result.EventName);
            Assert.Equal(100, result.Id);
            _eventRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Event>()), Times.Once);
            _eventAttendeeServiceMock.Verify(s => s.AddAttendeeAsync(100, 1), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_InvalidCreator_ThrowsKeyNotFoundException()
        {
            // Arrange: Twórca nie istnieje
            var eventCreateDto = new EventCreateDto
            {
                EventName = "Test Event",
                Description = "Test Description",
                SportType = "Bieganie",
                MaxParticipants = 10,
                Address = "Test Street 1",
                City = "Warszawa",
                Date = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1)),
                Time = TimeSpan.FromHours(10),
                CreatedByUserId = 99,
                Photo = null
            };

            _userRepositoryMock.Setup(r => r.GetByIdAsync(99))
                .ReturnsAsync((User)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _eventService.CreateAsync(eventCreateDto));
        }
    }
}
