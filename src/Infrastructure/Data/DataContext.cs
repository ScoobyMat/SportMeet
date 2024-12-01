using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }

        public override int SaveChanges()
        {
            SetEntityTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetEntityTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void SetEntityTimestamps()
        {
            var entries = ChangeTracker.Entries();

            foreach (var entry in entries)
            {
                if (entry.Entity is Event eventEntity)
                {
                    if (entry.State == EntityState.Added)
                    {
                        eventEntity.Created = DateTime.UtcNow;
                        eventEntity.LastModified = DateTime.UtcNow;
                    }

                    if (entry.State == EntityState.Modified)
                    {
                        eventEntity.LastModified = DateTime.UtcNow;
                    }
                }
            }
        }


    }
}
