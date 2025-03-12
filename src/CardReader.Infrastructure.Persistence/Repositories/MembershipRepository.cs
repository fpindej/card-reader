using CardReader.Application.Repositories;
using CardReader.Domain;
using Microsoft.EntityFrameworkCore;

namespace CardReader.Infrastructure.Persistence.Repositories;

internal class MembershipRepository : IMembershipRepository
{
    private readonly GymDoorDbContext _context;

    public MembershipRepository(GymDoorDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(Membership membership)
    {
        var existingCard = await _context.AccessCards
            .FirstOrDefaultAsync(c => c.CardNumber == membership.AccessCard.CardNumber);
        
        if (existingCard is not null)
        {
            return false;
        }

        var existingMembership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.CustomerId == membership.CustomerId && m.IsActive);
            
        if (existingMembership != null)
        {
            return false;
        }

        await _context.Memberships.AddAsync(membership);
        return true;
    }

    public async Task<AccessCard?> GetCardByNumberAsync(string cardNumber)
    {
        return await _context.AccessCards
            .AsNoTracking()
            .Include(c => c.Memberships)
            .FirstOrDefaultAsync(c => c.CardNumber == cardNumber);
    }
}