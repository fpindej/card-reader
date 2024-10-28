using System.ComponentModel.DataAnnotations;

namespace CardReader.Domain;

public class RfidCard
{
    [MaxLength(10)]
    public string Id { get; set; } = null!;
    
    public bool IsActive { get; set; }
}