using CardReader.Domain;

namespace CardReader.Application.Repositories;

public interface IMembershipRepository
{
    Task<Result<Membership>> CreateAsync(Membership membership);
    Task<Membership?> GetLatestMembershipAsync(int customerId);
    Task<Membership?> GetActiveByCardNumber(string cardNumber);
    Task<Result<Membership>> UpdateAsync(Membership membership);
    Task<Result> RevokeAsync(int membershipId);
    Task<List<string>> GetActiveCardNumbersAsync();
}