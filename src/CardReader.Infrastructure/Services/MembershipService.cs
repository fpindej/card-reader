using CardReader.Application;
using CardReader.Application.Repositories;
using CardReader.Application.Services;
using CardReader.Domain;

namespace CardReader.Infrastructure.Services;

internal class MembershipService : IMembershipService
{
    private readonly IMembershipRepository _membershipRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _uow;

    public MembershipService(IMembershipRepository membershipRepository,
        ICustomerRepository customerRepository,
        IUnitOfWork uow)
    {
        _membershipRepository = membershipRepository;
        _customerRepository = customerRepository;
        _uow = uow;
    }

    public async Task<Result<Membership>> CreateMembershipAsync(int customerId, string cardNumber, DateTime? expiresAt = null)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var existingCustomer = await _customerRepository.GetByIdAsync(customerId);
            if (existingCustomer is null)
            {
                await _uow.RollbackTransactionAsync();
                return Result<Membership>.Failure("Customer not found.");
            }
            
            var existingMembership = await _membershipRepository.GetLatestMembershipAsync(customerId);
            if (existingMembership?.IsActive is true)
            {
                await _uow.RollbackTransactionAsync();
                return Result<Membership>.Failure("Customer already has an active membership.");
            }
            
            var existingCard = await _membershipRepository.GetActiveByCardNumber(cardNumber);
            if (existingCard?.IsActive is true)
            {
                await _uow.RollbackTransactionAsync();
                return Result<Membership>.Failure("Card is already in use.");
            }

            var membership = new Membership
            {
                CustomerId = customerId,
                CardNumber = cardNumber,
                ExpiresAt = expiresAt
            };

            var created = await _membershipRepository.CreateAsync(membership);
            if (!created.IsSuccess)
            {
                await _uow.RollbackTransactionAsync();
                return created;
            }

            await _uow.CommitTransactionAsync();
            return created;
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            return Result<Membership>.Failure("Could not complete membership creation.");
        }
    }

    public async Task<Result> ValidateCardAccessAsync(string cardNumber)
    {
        try
        {
            var card = await _membershipRepository.GetActiveByCardNumber(cardNumber);
            return card?.IsActive is true
                ? Result.Success()
                : Result.Failure("Card is not valid.");
        }
        catch
        {
            return Result.Failure("Failed to validate card access.");
        }
    }

    public async Task<bool> ExtendMembershipAsync(int customerId, int daysToExtend)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var membership = await _membershipRepository.GetLatestMembershipAsync(customerId);
            if (membership is null)
            {
                await _uow.RollbackTransactionAsync();
                return false;
            }

            membership.ExpiresAt = membership is { IsActive: true, ExpiresAt: not null }
                ? membership.ExpiresAt.Value.AddDays(daysToExtend)
                : DateTime.UtcNow.AddDays(daysToExtend);

            var updated = await _membershipRepository.UpdateAsync(membership);
            if (!updated)
            {
                await _uow.RollbackTransactionAsync();
                return false;
            }

            await _uow.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            return false;
        }
    }
    
    public async Task<bool> RevokeMembershipAsync(int customerId)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var membership = await _membershipRepository.GetLatestMembershipAsync(customerId);
            if (membership is null || !membership.IsActive)
            {
                await _uow.RollbackTransactionAsync();
                return false;
            }

            var revoked = await _membershipRepository.RevokeAsync(membership.Id);
            if (!revoked)
            {
                await _uow.RollbackTransactionAsync();
                return false;
            }

            await _uow.CommitTransactionAsync();
            return true;
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            return false;
        }
    }
}