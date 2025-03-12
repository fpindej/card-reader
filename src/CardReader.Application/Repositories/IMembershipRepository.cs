using CardReader.Domain;

namespace CardReader.Application.Repositories;

public interface IMembershipRepository
{
    Task<bool> CreateAsync(Membership membership);
    Task<Membership?> GetLatestMembershipAsync(int customerId);
    Task<Membership?> GetCardByNumberAsync(string cardNumber);
    Task<bool> UpdateAsync(Membership membership);
    Task<bool> RevokeAsync(int membershipId);
}