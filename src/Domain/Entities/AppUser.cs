using Domain.Common;

namespace Domain.Entities;

public class AppUser : BaseDomainEntity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; } 
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public required string Gender { get; set; }
    public string? Introduction { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public Photo? ProfilePhoto { get; set; }
    public int GetAge()
    {
        return DateOfBirth.CalculateAge();
    }
}
