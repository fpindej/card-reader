namespace CardReader.WebApi.Dtos;

public record MembershipCreateRequest(int CustomerId, string CardNumber, DateTime? ExpiresAt = null);