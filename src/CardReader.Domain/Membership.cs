namespace CardReader.Domain;

public class Membership
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    
    public string CardNumber { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive => ExpiresAt.HasValue && ExpiresAt.Value > DateTime.UtcNow;

    public virtual Customer Customer { get; set; } = null!;
}