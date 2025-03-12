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
        var existingCard = await _context.Memberships
            .AnyAsync(m => m.CardNumber == membership.CardNumber);

        if (existingCard)
        {
            return false;
        }

        var existingMembership = await _context.Memberships
            .Where(m => m.CustomerId == membership.CustomerId)
            .Where(m => m.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync();

        if (existingMembership != null)
        {
            return false;
        }

        await _context.Memberships.AddAsync(membership);
        return true;
    }

    public async Task<Membership?> GetLatestMembershipAsync(int customerId)
    {
        return await _context.Memberships
            .Include(m => m.Customer)
            .Where(m => m.CustomerId == customerId)
            .OrderByDescending(m => m.ExpiresAt ?? DateTime.MinValue)
            .FirstOrDefaultAsync();
    }

    public async Task<Membership?> GetCardByNumberAsync(string cardNumber)
    {
        return await _context.Memberships
            .Include(m => m.Customer)
            .FirstOrDefaultAsync(m => m.CardNumber == cardNumber);
    }

    public async Task<bool> UpdateAsync(Membership membership)
    {
        var existingMembership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.Id == membership.Id);

        if (existingMembership is null)
        {
            return false;
        }

        existingMembership.ExpiresAt = membership.ExpiresAt;
        return true;
    }
    
    public async Task<bool> RevokeAsync(int membershipId)
    {
        var membership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.Id == membershipId);

        if (membership is null)
        {
            return false;
        }

        membership.ExpiresAt = DateTime.UtcNow;
        return true;
    }
}