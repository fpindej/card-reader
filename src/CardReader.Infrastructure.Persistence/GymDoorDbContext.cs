using CardReader.Domain;
using Microsoft.EntityFrameworkCore;

namespace CardReader.Infrastructure.Persistence;

internal class GymDoorDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; init; }

    public DbSet<AccessCard> AccessCards { get; init; }

    public DbSet<Membership> Memberships { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDoorDbContext).Assembly);

        modelBuilder.Entity<User>(builder =>
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .UseIdentityColumn();

            builder.Property(b => b.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(b => b.YearOfBirth)
                .IsRequired();
        });

        modelBuilder.Entity<AccessCard>(builder =>
        {
            builder.HasKey(ac => ac.Id);

            builder.Property(ac => ac.Id)
                .UseIdentityColumn();

            builder.Property(ac => ac.CardNumber)
                .IsRequired()
                .HasMaxLength(64);

            builder.HasIndex(ac => ac.CardNumber)
                .IsUnique();
        });

        modelBuilder.Entity<Membership>(builder =>
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .UseIdentityColumn();

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(m => m.User)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(m => m.AccessCard)
                .WithMany(r => r.Memberships)
                .HasForeignKey(m => m.AccessCardId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}