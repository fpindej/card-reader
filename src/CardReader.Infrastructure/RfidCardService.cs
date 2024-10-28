using CardReader.Application;
using CardReader.Application.Repositories;
using CardReader.Domain;
using Microsoft.Extensions.Logging;

namespace CardReader.Infrastructure;

public class RfidCardService : IRfidCardService
{
    private readonly ILogger<RfidCardService> _logger;
    private readonly IRfidCardRepository _rfidCardRepository;

    public RfidCardService(ILogger<RfidCardService> logger, IRfidCardRepository rfidCardRepository)
    {
        _logger = logger;
        _rfidCardRepository = rfidCardRepository;
    }

    public async Task<RfidCard> CreateAsync(RfidCard rfidCard)
    {
        _logger.LogInformation("Creating RFID card...");

        return await _rfidCardRepository.CreateAsync(rfidCard);
    }

    public async Task<RfidCard?> GetByIdAsync(string id)
    {
        _logger.LogInformation("Getting RFID card by ID...");

        return await _rfidCardRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<RfidCard>> GetAllActiveAsync()
    {
        _logger.LogInformation("Getting all active RFID cards...");

        return await _rfidCardRepository.GetAllActiveAsync();
    }

    public async Task<bool> ActivateAsync(string id)
    {
        _logger.LogInformation("Activating RFID card...");

        return await _rfidCardRepository.ActivateAsync(id);
    }

    public async Task<bool> DeactivateAsync(string id)
    {
        _logger.LogInformation("Deactivating RFID card...");

        return await _rfidCardRepository.DeactivateAsync(id);
    }

    public async Task<bool> UpdateAsync(RfidCard rfidCard)
    {
        _logger.LogInformation("Updating RFID card...");

        return await _rfidCardRepository.UpdateAsync(rfidCard);
    }

    public async Task<bool> DeleteAsync(string id)
    {
        _logger.LogInformation("Deleting RFID card...");

        return await _rfidCardRepository.DeleteAsync(id);
    }
}