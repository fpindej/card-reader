namespace CardReader.Domain;

public class Customer
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateTime YearOfBirth { get; set; }

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
}