namespace CardReader.WebApi.Dtos;

public record CustomerUpdateRequest(int Id, string? FirstName, string? LastName, string? Email);