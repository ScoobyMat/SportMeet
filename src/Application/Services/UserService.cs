using Application.DTOs.User;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null) return false;

        _userRepository.Delete(user);
        return true;
    }

    public async Task<UserDto?> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);

        if (user == null)
        {
            return null;
        }

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }

    public async Task<UserDto?> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);

        if (user == null)
        {
            return null;
        }

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var users = await _userRepository.GetUsers();

        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        return userDtos;
    }

    public async Task<bool> UpdateUser(UserUpdateDto memberUpdateDto)
    {
        var user = await _userRepository.GetUserById(memberUpdateDto.Id);

        if (user == null)
        {
            return false;
        }

        _mapper.Map(memberUpdateDto, user);
        _userRepository.Update(user);

        return true;
    }
}
