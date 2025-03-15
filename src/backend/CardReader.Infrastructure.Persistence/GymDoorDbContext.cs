using CardReader.Domain;
using Microsoft.EntityFrameworkCore;

namespace CardReader.Infrastructure.Persistence;

internal class GymDoorDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Customer> Customers { get; init; }

    public DbSet<Membership> Memberships { get; init; }

    public DbSet<AccessLog> AccessLogs { get; init; }
    
    public DbSet<DeviceHealth> DeviceHealths { get; init; }

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

        modelBuilder.Entity<AccessLog>(builder =>
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .UseIdentityColumn();

            builder.Property(a => a.EventDateTime)
                .IsRequired();

            builder.Property(a => a.CardNumber)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(a => a.IsSuccessful)
                .IsRequired();
        });

        modelBuilder.Entity<DeviceHealth>(builder =>
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.Id)
                .UseIdentityColumn();

            builder.Property(d => d.DeviceId)
                .IsRequired();

            builder.Property(d => d.MaxAllocHeap)
                .IsRequired();

            builder.Property(d => d.MinFreeHeap)
                .IsRequired();

            builder.Property(d => d.FreeHeap)
                .IsRequired();

            builder.Property(d => d.Uptime)
                .IsRequired();

            builder.Property(d => d.FreeSketchSpace)
                .IsRequired();
            
            builder.Property(d => d.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });
    }
}
