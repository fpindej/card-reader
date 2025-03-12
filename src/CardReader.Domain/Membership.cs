namespace CardReader.Domain;

public class Membership
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int AccessCardId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; }

    public virtual Customer Customer { get; set; } = null!;
    public virtual AccessCard AccessCard { get; set; } = null!;
}