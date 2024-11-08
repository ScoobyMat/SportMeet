using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class RegisterDto : IMap
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string? Gender {  get; set; }
        [Required]
        public string? DateOfBirth { get; set; }
        [Required]
        public string? City { get; set; }
        [Required]
        public string? Country { get; set; }

        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(50,MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterDto, AppUser>();
            profile.CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
        }
    }
}
