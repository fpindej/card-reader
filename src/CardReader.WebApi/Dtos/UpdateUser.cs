namespace CardReader.WebApi.Dtos;

public record UpdateUserRequest(int Id, string FirstName, string LastName, ushort YearOfBirth);