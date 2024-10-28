using CardReader.Application.Repositories;
using CardReader.Domain;
using CardReader.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CardReader.Infrastructure.Persistence.Repositories;

internal class RfidCardRepository : IRfidCardRepository
{
    private readonly ILogger<RfidCardRepository> _logger;
    private readonly GymDoorDbContext _context;

    public RfidCardRepository(ILogger<RfidCardRepository> logger, GymDoorDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<RfidCard> CreateAsync(RfidCard rfidCard)
    {
        var cardModel = rfidCard.ToModel();

        _ = await _context.RfidCards.AddAsync(cardModel);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Created new RFID card with ID: {cardId}", cardModel.Id);
        
        return cardModel.ToDomain();
    }

    public async Task<RfidCard?> GetByIdAsync(string cardId)
    {
        var cardModel = await _context.RfidCards.FirstOrDefaultAsync(x => x.Id == cardId);
        
        return cardModel?.ToDomain();
    }

    public async Task<IEnumerable<RfidCard>> GetAllActiveAsync()
    {
        var activeCards = await _context.RfidCards
            .Where(c => c.IsActive)
            .ToListAsync();

        return activeCards.Select(c => c.ToDomain());
    }

    public async Task<bool> ActivateAsync(string cardId)
    {
        var cardModel = await _context.RfidCards.FirstOrDefaultAsync(x => x.Id == cardId);

        if (cardModel is null)
            return false;

        cardModel.IsActive = true;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Activated RFID card with ID: {cardId}", cardId);
        
        return true;
    }

    public async Task<bool> DeactivateAsync(string cardId)
    {
        var cardModel = await _context.RfidCards.FirstOrDefaultAsync(x => x.Id == cardId);

        if (cardModel is null)
            return false;

        cardModel.IsActive = false;
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deactivated RFID card with ID: {cardId}", cardId);
        
        return true;
    }

    public async Task<bool> UpdateAsync(RfidCard rfidCard)
    {
        var cardModel = await _context.RfidCards.FirstOrDefaultAsync(x => x.Id == rfidCard.Id);

        if (cardModel is null)
            return false;

        cardModel.UserId = rfidCard.UserId;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(string cardId)
    {
        var cardModel = await _context.RfidCards.FindAsync(cardId);

        if (cardModel is null)
            return false;

        _context.RfidCards.Remove(cardModel);
        await _context.SaveChangesAsync();
        return true;
    }
}