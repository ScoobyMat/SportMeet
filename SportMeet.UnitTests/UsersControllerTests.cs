using API.Controllers;
using Application.Dtos.UserDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace SportMeet.UnitTests
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly UsersController _controller;

        public UsersControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UsersController(_userServiceMock.Object);
        }

        [Fact]
        public async Task GetUser_ReturnsOkResult_WithUserDto()
        {
            var userDto = new UserDto
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                UserName = "Jano",
                Email = "jan.kowalski@example.com",
                Password = "hashed",
                Gender = "Mężczyzna",
                Age = 20,
                City = "Warszawa",
                Country = "Polska",
            };

            _userServiceMock.Setup(s => s.GetUserByIdAsync(1))
                .ReturnsAsync(userDto);

            var result = await _controller.GetUser(1);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsType<UserDto>(okResult.Value);
            Assert.Equal("Jan", returnedUser.FirstName);
        }

        [Fact]
        public async Task GetUser_NonexistentId_ReturnsNotFound()
        {
            _userServiceMock.Setup(s => s.GetUserByIdAsync(99))
                .ThrowsAsync(new KeyNotFoundException("User not found."));

            var result = await _controller.GetUser(99);

            Assert.IsType<NotFoundObjectResult>(result.Result);
        }
    }
}
