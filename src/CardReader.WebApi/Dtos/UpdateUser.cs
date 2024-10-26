namespace CardReader.WebApi.Dtos;

public record UpdateUserRequest(Guid Id, string FirstName, string LastName, ushort YearOfBirth);