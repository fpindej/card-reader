using System.ComponentModel.DataAnnotations;

namespace CardReader.WebApi.Dtos;

public record CustomerUpdateRequest(
    [Required] int Id,
    [MaxLength(50)] string? FirstName,
    [MaxLength(50)] string? LastName,
    [EmailAddress] string? Email);