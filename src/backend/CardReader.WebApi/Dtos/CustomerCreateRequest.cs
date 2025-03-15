using System.ComponentModel.DataAnnotations;

namespace CardReader.WebApi.Dtos;

public record CustomerCreateRequest(
    [Required] [MaxLength(50)] string FirstName,
    [Required] [MaxLength(50)] string LastName,
    [Required] [EmailAddress] string Email);