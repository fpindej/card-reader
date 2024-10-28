using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardReader.Infrastructure.Persistence.Models;

internal class RfidCard : IEntityTypeConfiguration<Models.RfidCard>
{
    [MaxLength(10)]
    [Description("Represents the unique RFID card ID used to detect which card has been scanned.")]
    public string Id { get; set; } = null!;

    public bool IsActive { get; set; }

    [ForeignKey(nameof(User))]
    public int? UserId { get; set; }

    public virtual Models.User? User { get; set; }

    public void Configure(EntityTypeBuilder<RfidCard> builder)
    {
        builder.HasKey(rc => rc.Id);
        
        builder.Property(rc => rc.Id)
            .IsRequired()
            .HasMaxLength(10)
            .ValueGeneratedNever();
        
        builder.Property(rc => rc.IsActive)
            .IsRequired();
    }
}