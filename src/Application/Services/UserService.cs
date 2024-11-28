using Application.Dtos.UserDtos;
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

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return false;
        }

        await _userRepository.DeleteAsync(user);
        return true;
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);

        if (user == null)
        {
            return null;
        }

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return null;
        }

        var userDto = _mapper.Map<UserDto>(user);
        return userDto;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        return userDtos;
    }

    public async Task<bool> UpdateUserAsync(UserUpdateDto memberUpdateDto)
    {
        var user = await _userRepository.GetByIdAsync(memberUpdateDto.Id);

        if (user == null)
        {
            return false;
        }

        _mapper.Map(memberUpdateDto, user);
        await _userRepository.UpdateAsync(user);
        return true;
    }
}
