using System;
using Application.DTOs.User;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetUsers();
    Task<UserDto?> GetUserById(int id);
    Task<UserDto?> GetUserByEmail(string email);
    Task<bool> UpdateUser(UserUpdateDto updateDto);
    Task<bool> DeleteUser(int id);

}
