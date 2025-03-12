namespace CardReader.WebApi.Dtos;

public record CreateMembershipRequest(int UserId, DateTime StartDate, DateTime EndDate);

public record CreateMembershipResponse(int Id, int UserId, DateTime StartDate, DateTime EndDate, bool IsActive);

public record MembershipResponse(int Id, int UserId, DateTime StartDate, DateTime EndDate, bool IsActive);
