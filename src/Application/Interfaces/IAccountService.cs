using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAccountService
    {
        Task<UserDto> Register(RegisterDto registerDto);
        Task<UserDto> Login(LoginDto loginDto);
    }
}
