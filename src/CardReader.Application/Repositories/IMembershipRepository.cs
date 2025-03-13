using CardReader.Domain;

namespace CardReader.Application.Repositories;

public interface IMembershipRepository
{
    Task<Result<Membership>> CreateAsync(Membership membership);
    Task<Membership?> GetLatestMembershipAsync(int customerId);
    Task<Membership?> GetActiveByCardNumber(string cardNumber);
    Task<Result> UpdateAsync(Membership membership);
    Task<bool> RevokeAsync(int membershipId);
}