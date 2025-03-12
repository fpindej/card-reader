using CardReader.Domain;

namespace CardReader.Application.Repositories;

public interface IMembershipRepository
{
    Task<bool> CreateAsync(Membership membership);
    Task<AccessCard?> GetCardByNumberAsync(string cardNumber);
}