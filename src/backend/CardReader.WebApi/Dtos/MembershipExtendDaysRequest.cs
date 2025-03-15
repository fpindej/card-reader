namespace CardReader.WebApi.Dtos;

public record MembershipExtendDaysRequest(int CustomerId, int DaysToExtend);