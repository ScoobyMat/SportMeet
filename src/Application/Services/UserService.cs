using Application.Dtos.UserDtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IPhotoService photoService, IMapper mapper)
        {
            _userRepository = userRepository;
            _photoService = photoService;
            _mapper = mapper;
        }


        public async Task DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User not found.");
            }

            await _userRepository.DeleteAsync(user);
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null)
            {
                throw new KeyNotFoundException($"User not found.");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User not found.");
            }

            var userDto = _mapper.Map<UserDto>(user);
            return userDto;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();

            if (users == null)
            {
                throw new InvalidOperationException("No users found.");
            }

            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return userDtos;
        }

        public async Task UpdateUserAsync(UserUpdateDto userUpdateDto)
        {
            var user = await _userRepository.GetByIdAsync(userUpdateDto.Id);

            if (user == null)
            {
                throw new KeyNotFoundException($"User not found.");
            }

            if (userUpdateDto.Photo != null)
            {
                if (!string.IsNullOrEmpty(user.PhotoPublicId))
                {
                    await _photoService.DeletePhotoAsync(user.PhotoPublicId);
                }

                var uploadResult = await _photoService.AddPhotoAsync(userUpdateDto.Photo, "user");

                user.PhotoUrl = uploadResult.Url.ToString();
                user.PhotoPublicId = uploadResult.PublicId;
            }
            else
            {
                if (!string.IsNullOrEmpty(user.PhotoPublicId))
                {
                    await _photoService.DeletePhotoAsync(user.PhotoPublicId);

                    user.PhotoUrl = null;
                    user.PhotoPublicId = null;
                }
            }

            _mapper.Map(userUpdateDto, user);

            await _userRepository.UpdateAsync(user);
        }

    }
}
