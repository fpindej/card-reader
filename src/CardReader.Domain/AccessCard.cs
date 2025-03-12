namespace CardReader.Domain;

public class AccessCard
{
    public int Id { get; set; }

    public string CardNumber { get; set; } = null!;

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();

    public bool HasValidMembership()
    {
        var now = DateTime.UtcNow;
        return Memberships.Any(m => m.IsActive && (!m.ExpiresAt.HasValue || m.ExpiresAt.Value > now));
    }
}