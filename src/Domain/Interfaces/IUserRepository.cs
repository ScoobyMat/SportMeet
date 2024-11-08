using Domain.Entities;

namespace Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<AppUser>> GetUsers();
    Task<AppUser?> GetUserById(int id);
    Task<AppUser?> GetUserByEmail(string email);
    Task<bool> UserExistsByEmail(string email);
    Task AddUser(AppUser user);
    void Update(AppUser user);
    void Delete(AppUser user);
    Task<bool> SaveAll();

}
