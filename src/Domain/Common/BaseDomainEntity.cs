using System;

namespace Domain.Entities;

public abstract class BaseDomainEntity
{
    public int Id { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastModified { get; set;} = DateTime.UtcNow;
}
