using Application.Dtos.UserDtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Pobranie wszystkich u�ytkownik�w aplikacji
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Pobranie konkretnego u�ytkownika na podstawie jego ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Pobranie konkretnego u�ytkownika na podstawie jego username
        /// </summary>
        [HttpGet("username/{username}")]
        public async Task<ActionResult<UserDto>> GetUserByUsername(string username)
        {
            try
            {
                var userDto = await _userService.GetUserByUsernameAsync(username);
                return Ok(userDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Aktualizacja danych u�ytkownika
        /// </summary>
        [HttpPut]
        public async Task<ActionResult<UserDto>> UpdateUser(UserUpdateDto userUpdateDto)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserAsync(userUpdateDto);

                return Ok(updatedUser);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Usuni�cie danego u�ytkownika
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
