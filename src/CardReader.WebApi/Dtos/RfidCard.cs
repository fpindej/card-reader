namespace CardReader.WebApi.Dtos;

public record CreateRfidCardRequest(string CardId, int UserId);

public record CreateRfidCardResponse(string CardId, bool IsActive, int UserId);

public record RfidCardResponse(string CardId, bool IsActive, int? UserId);