﻿using CardReader.Application.Repositories;
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

    public async Task<Result<Membership>> CreateAsync(Membership membership)
    {
        try
        {
            await _context.Memberships.AddAsync(membership);
            return Result<Membership>.Success(membership);
        }
        catch
        {
            return Result<Membership>.Failure("Failed to create membership.");
        }
    }

    public async Task<Membership?> GetLatestMembershipAsync(int customerId)
    {
        return await _context.Memberships
            .Include(m => m.Customer)
            .Where(m => m.CustomerId == customerId)
            .OrderByDescending(m => m.ExpiresAt ?? DateTime.MinValue)
            .FirstOrDefaultAsync();
    }

    public async Task<Membership?> GetActiveByCardNumber(string cardNumber)
    {
        var memberships = await _context.Memberships
            .Include(m => m.Customer)
            .Where(m => m.CardNumber == cardNumber)
            .ToListAsync();

        return memberships.FirstOrDefault(m => m.IsActive);
    }

    public async Task<Result<Membership>> UpdateAsync(Membership membership)
    {
        var existingMembership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.Id == membership.Id);

        if (existingMembership is null)
        {
            return Result<Membership>.Failure("Membership not found.");
        }

        existingMembership.ExpiresAt = membership.ExpiresAt;
        return Result<Membership>.Success(existingMembership);
    }
    
    public async Task<Result> RevokeAsync(int membershipId)
    {
        var membership = await _context.Memberships
            .FirstOrDefaultAsync(m => m.Id == membershipId);

        if (membership is null)
        {
            return Result.Failure("Membership not found.");
        }

        membership.ExpiresAt = DateTime.UtcNow;
        return Result.Success();
    }

    public async Task<List<string>> GetActiveCardNumbersAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Memberships
            .Where(m => m.ExpiresAt == null || m.ExpiresAt > now)
            .Select(m => m.CardNumber)
            .ToListAsync();
    }
    
    public async Task<List<string>> GetAllCards()
    {
        return await _context.Memberships
            .Select(m => m.CardNumber)
            .Distinct()
            .ToListAsync();
    }
}