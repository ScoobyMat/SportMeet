using System;
using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByEmailAsync(string email);
    void Update(AppUser user);
    Task<bool> SaveAllAsync();

}
