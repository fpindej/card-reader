using Microsoft.EntityFrameworkCore;

namespace CardReader.Infrastructure.Persistence;

internal class GymDoorDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Models.User> Users { get; init; }
    
    public DbSet<Models.RfidCard> RfidCards { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDoorDbContext).Assembly);
    }
}