using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Group> Groups { get; set; }

        public override int SaveChanges()
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

            return base.SaveChanges();
        }


    }
}
