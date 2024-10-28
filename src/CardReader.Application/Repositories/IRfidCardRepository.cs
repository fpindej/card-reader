using CardReader.Domain;

namespace CardReader.Application.Repositories;

public interface IRfidCardRepository
{
    Task<RfidCard> CreateAsync(RfidCard rfidCard);
    Task<RfidCard?> GetByIdAsync(string cardId);
    Task<IEnumerable<RfidCard>> GetAllActiveAsync();
    Task<bool> ActivateAsync(string cardId);
    Task<bool> DeactivateAsync(string cardId);
    Task<bool> UpdateAsync(RfidCard rfidCard);
    Task<bool> DeleteAsync(string cardId);
}