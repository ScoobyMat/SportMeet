using Application.DTOs;
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
        return await _userRepository.SaveAll();
    }

    public async Task<MemberDto?> GetUserByEmail(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);

        if (user == null)
        {
            return null;
        }

        var userDto = _mapper.Map<MemberDto>(user);
        return userDto;
    }

    public async Task<MemberDto?> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);

        if (user == null)
        {
            return null;
        }

        var userDto = _mapper.Map<MemberDto>(user);
        return userDto;
    }

    public async Task<IEnumerable<MemberDto>> GetUsers()
    {
        var users = await _userRepository.GetUsers();

        var userDtos = _mapper.Map<IEnumerable<MemberDto>>(users);
        return userDtos;
    }

    public async Task<bool> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var user = await _userRepository.GetUserById(memberUpdateDto.Id);

        if (user == null)
        {
            return false;
        }

        _mapper.Map(memberUpdateDto, user);
        _userRepository.Update(user);

       return await _userRepository.SaveAll();
    }
}
