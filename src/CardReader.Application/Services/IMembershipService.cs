using CardReader.Domain;

namespace CardReader.Application.Services;

public interface IMembershipService
{
    Task<Result<Membership>> CreateMembershipAsync(int customerId, string cardNumber, DateTime? expiresAt = null);
    Task<Result> ValidateCardAccessAsync(string cardNumber);
    Task<Result> ExtendMembershipAsync(int customerId, int daysToExtend);
    Task<bool> RevokeMembershipAsync(int customerId);
}