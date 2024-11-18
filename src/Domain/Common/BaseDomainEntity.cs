using System;

namespace Domain.Entities;

public abstract class BaseDomainEntity
{
    public int Id { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastModified { get; set;}
}
