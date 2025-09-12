using Application.Dtos.UserDtos;
using AutoMapper;
using Application.Mappings;
using Domain.Entities;

namespace Application.Dtos.Authentication
{
    public class AuthResponseDto
    {
        public required string Token { get; set; }
    }
}