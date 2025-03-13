using CardReader.Domain;
using Microsoft.EntityFrameworkCore;

namespace CardReader.Infrastructure.Persistence;

internal class GymDoorDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; init; }

    public DbSet<Membership> Memberships { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymDoorDbContext).Assembly);

        modelBuilder.Entity<Customer>(builder =>
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
        });

        modelBuilder.Entity<Membership>(builder =>
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .UseIdentityColumn();

            builder.Property(m => m.CardNumber)
                .HasMaxLength(64);

            builder.Property(m => m.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.HasOne(m => m.Customer)
                .WithMany(u => u.Memberships)
                .HasForeignKey(m => m.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}