using System;
using Application.DTOs;
using Application.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    public UserDto GetUserById(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UserDto> GetUsers()
    {
        throw new NotImplementedException();
    }
}
