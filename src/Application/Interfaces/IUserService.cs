using System;
using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    IEnumerable<UserDto> GetUsers();
    UserDto GetUserById(int id);
}
