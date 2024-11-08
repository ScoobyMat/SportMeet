using System;
using Application.DTOs;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService
{
    Task<IEnumerable<MemberDto>> GetUsers();
    Task<MemberDto?> GetUserById(int id);
    Task<MemberDto?> GetUserByEmail(string email);
    Task<bool> UpdateUser(MemberUpdateDto updateDto);
    Task<bool> DeleteUser(int id);

}
