using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardReader.Infrastructure.Persistence.Models;

internal class User : IEntityTypeConfiguration<Models.User>
{
    public int Id { get; init; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public ushort YearOfBirth { get; set; }
    
    public string? Notes { get; set; }
    
    public virtual RfidCard? RfidCard { get; set; }

    public void Configure(EntityTypeBuilder<User> builder)
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

        builder.Property(u => u.Notes)
            .IsRequired(false)
            .HasMaxLength(200);
        
        builder.HasOne(u => u.RfidCard)
            .WithOne(rc => rc.User)
            .HasForeignKey<RfidCard>(rc => rc.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}