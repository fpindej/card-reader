using System.ComponentModel.DataAnnotations;

namespace CardReader.Domain;

public class Customer
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public virtual ICollection<Membership> Memberships { get; set; } = new List<Membership>();
}