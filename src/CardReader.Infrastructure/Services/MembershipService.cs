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
                return Result<Membership>.Failure($"Card {existingCard.CardNumber} is already used by customerId {existingCard.CustomerId}.");
            }

            var membership = new Membership
            {
                CustomerId = customerId,
                CardNumber = cardNumber,
                ExpiresAt = expiresAt
            };

            var result = await _membershipRepository.CreateAsync(membership);
            if (result.IsSuccess)
            {
                await _uow.CommitTransactionAsync();
            }
            else
            {
                await _uow.RollbackTransactionAsync();
            }

            return result;
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

    public async Task<Result> ExtendMembershipAsync(int customerId, int daysToExtend)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var existingMembership = await _membershipRepository.GetLatestMembershipAsync(customerId);
            if (existingMembership is null)
            {
                await _uow.RollbackTransactionAsync();
                return Result.Failure("Membership does not exist. Nothing to extend.");
            }
            
            var existingCard = await _membershipRepository.GetActiveByCardNumber(existingMembership.CardNumber);
            if (existingCard?.IsActive is true)
            {
                await _uow.RollbackTransactionAsync();
                return Result.Failure($"Card {existingCard.CardNumber} is already used by customerId {existingCard.CustomerId}.");
            }

            existingMembership.ExpiresAt = existingMembership is { IsActive: true, ExpiresAt: not null }
                ? existingMembership.ExpiresAt.Value.AddDays(daysToExtend)
                : DateTime.UtcNow.AddDays(daysToExtend);

            var result = await _membershipRepository.UpdateAsync(existingMembership);
            if (result.IsSuccess)
            {
                await _uow.CommitTransactionAsync();
            }
            else
            {
                await _uow.RollbackTransactionAsync();
            }

            return result;
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            return Result.Failure("Could not extend membership.");
        }
    }
    
    public async Task<Result> RevokeMembershipAsync(int customerId)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var existingMembership = await _membershipRepository.GetLatestMembershipAsync(customerId);
            if (existingMembership is null)
            {
                await _uow.RollbackTransactionAsync();
                return Result.Failure("Membership does not exist. Nothing to revoke.");
            }
            
            if (existingMembership.IsActive is false)
            {
                await _uow.RollbackTransactionAsync();
                return Result.Failure($"No active membership found for customerId {customerId}.");
            }

            var result = await _membershipRepository.RevokeAsync(existingMembership.Id);
            if (result.IsSuccess)
            {
                await _uow.CommitTransactionAsync();
            }
            else
            {
                await _uow.RollbackTransactionAsync();
            }

            return Result.Success();
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            return Result.Failure("Could not complete membership revocation.");
        }
    }
}