using CardReader.Domain;

namespace CardReader.Application.Services;

public interface IMembershipService
{
    Task<Result<Membership>> CreateMembershipAsync(int customerId, string cardNumber, DateTime? expiresAt = null);
    Task<Result<Membership>> CheckMembershipByCardNumberAsync(string cardNumber);
    Task<Result<Membership>> ExtendMembershipByDaysAsync(int customerId, int daysToExtend);
    Task<Result<Membership>> ExtendMembershipByMonthsAsync(int customerId, int monthsToExtend);
    Task<Result> RevokeMembershipAsync(int customerId);
    Task<Result<List<string>>> GetActiveCardNumbersAsync();
}