using CardReader.Application;
using CardReader.Domain;
using CardReader.Application.Repositories;
using Microsoft.Extensions.Logging;

namespace CardReader.Infrastructure;

public class MembershipService : IMembershipService
{
    private readonly ILogger<MembershipService> _logger;
    private readonly IRepository<Membership> _membershipRepository;

    public MembershipService(ILogger<MembershipService> logger, IRepository<Membership> membershipRepository)
    {
        _logger = logger;
        _membershipRepository = membershipRepository;
    }

    public async Task<bool> ActivateAsync(int membershipId)
    {
        var membership = await _membershipRepository.GetByIdAsync(membershipId);
        if (membership == null)
        {
            _logger.LogWarning("Membership with ID {membershipId} not found.", membershipId);
            return false;
        }

        membership.Activate();
        await _membershipRepository.UpdateAsync(membership);
        _logger.LogInformation("Activated membership with ID {membershipId}.", membershipId);
        return true;
    }

    public async Task<bool> DeactivateAsync(int membershipId)
    {
        var membership = await _membershipRepository.GetByIdAsync(membershipId);
        if (membership == null)
        {
            _logger.LogWarning("Membership with ID {membershipId} not found.", membershipId);
            return false;
        }

        membership.Deactivate();
        await _membershipRepository.UpdateAsync(membership);
        _logger.LogInformation("Deactivated membership with ID {membershipId}.", membershipId);
        return true;
    }

    public async Task<bool> ExtendAsync(int membershipId, int days)
    {
        var membership = await _membershipRepository.GetByIdAsync(membershipId);
        if (membership == null)
        {
            _logger.LogWarning("Membership with ID {membershipId} not found.", membershipId);
            return false;
        }

        membership.Extend(days);
        await _membershipRepository.UpdateAsync(membership);
        _logger.LogInformation("Extended membership with ID {membershipId} by {days} days.", membershipId, days);
        return true;
    }

    public async Task<bool> RevokeAsync(int membershipId)
    {
        var membership = await _membershipRepository.GetByIdAsync(membershipId);
        if (membership == null)
        {
            _logger.LogWarning("Membership with ID {membershipId} not found.", membershipId);
            return false;
        }

        membership.Revoke();
        await _membershipRepository.UpdateAsync(membership);
        _logger.LogInformation("Revoked membership with ID {membershipId}.", membershipId);
        return true;
    }
}
