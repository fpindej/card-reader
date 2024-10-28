using CardReader.Domain;

namespace CardReader.Application;

public interface IRfidCardService
{
    Task<RfidCard> CreateAsync(RfidCard rfidCard);
    Task<RfidCard?> GetByIdAsync(string id);
    Task<IEnumerable<RfidCard>> GetAllActiveAsync();
    Task<bool> ActivateAsync(string id);
    Task<bool> DeactivateAsync(string id);
    Task<bool> UpdateAsync(RfidCard rfidCard);
    Task<bool> DeleteAsync(string id);
}