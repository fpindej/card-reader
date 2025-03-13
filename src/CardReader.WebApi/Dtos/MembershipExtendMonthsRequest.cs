namespace CardReader.WebApi.Dtos;

public record MembershipExtendMonthsRequest(int CustomerId, int MonthsToExtend = 1);