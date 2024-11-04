using System;
using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public Task<AppUser?> GetUserByEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<AppUser?> GetUserByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        throw new NotImplementedException();
    }

    public Task<bool> SaveAllAsync()
    {
        throw new NotImplementedException();
    }

    public void Update(AppUser user)
    {
        throw new NotImplementedException();
    }
}
