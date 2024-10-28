using System.ComponentModel.DataAnnotations;

namespace CardReader.Domain;

public class User
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required ushort YearOfBirth { get; set; }
    
    [MaxLength(200)]
    public string? Notes { get; set; }
    
    public RfidCard? RfidCard { get; set; }
}