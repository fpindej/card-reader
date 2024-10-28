namespace CardReader.WebApi.Dtos;

public record CreateUserRequest(string FirstName, string LastName, ushort YearOfBirth, string? Notes);

public record CreateUserResponse(int Id, string FirstName, string LastName, ushort YearOfBirth, string? Notes);