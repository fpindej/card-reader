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
                return await RollbackWithFailure<Membership>("Customer not found.");
            }

            var existingMembership = await _membershipRepository.GetLatestMembershipAsync(customerId);
            if (existingMembership?.IsActive is true)
            {
                return await RollbackWithFailure<Membership>("Customer already has an active membership.");
            }

            var existingCard = await _membershipRepository.GetActiveByCardNumber(cardNumber);
            if (existingCard?.IsActive is true)
            {
                return await RollbackWithFailure<Membership>($"Card {existingCard.CardNumber} is already used by customerId {existingCard.CustomerId}.");
            }

            var membership = new Membership
            {
                CustomerId = customerId,
                CardNumber = cardNumber,
                ExpiresAt = expiresAt
            };

            var result = await _membershipRepository.CreateAsync(membership);
            return await CommitOrRollback(result);
        }
        catch
        {
            return await RollbackWithFailure<Membership>("Could not complete membership creation.");
        }
    }

    public async Task<Result<Membership>> CheckMembershipByCardNumberAsync(string cardNumber)
    {
        try
        {
            var membership = await _membershipRepository.GetActiveByCardNumber(cardNumber);
            return membership?.IsActive is true
                ? Result<Membership>.Success(membership)
                : Result<Membership>.Failure("Card is not valid.");
        }
        catch
        {
            return Result<Membership>.Failure("Failed to validate card access.");
        }
    }

    public Task<Result<Membership>> ExtendMembershipByDaysAsync(int customerId, int daysToExtend)
    {
        return ExtendMembershipAsync(customerId, date => date.AddDays(daysToExtend));
    }

    public Task<Result<Membership>> ExtendMembershipByMonthsAsync(int customerId, int monthsToExtend)
    {
        return ExtendMembershipAsync(customerId, date => date.AddMonths(monthsToExtend));
    }


    public async Task<Result> RevokeMembershipAsync(int customerId)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var existingMembership = await _membershipRepository.GetLatestMembershipAsync(customerId);
            if (existingMembership is null)
            {
                return await RollbackWithFailure("Membership does not exist. Nothing to revoke.");
            }

            if (existingMembership.IsActive is false)
            {
                return await RollbackWithFailure($"No active membership found for customerId {customerId}.");
            }

            var result = await _membershipRepository.RevokeAsync(existingMembership.Id);
            return await CommitOrRollback(result);
        }
        catch
        {
            return await RollbackWithFailure("Could not complete membership revocation.");
        }
    }

    public async Task<Result<List<string>>> GetActiveCardNumbersAsync()
    {
        try
        {
            var activeCards = await _membershipRepository.GetActiveCardNumbersAsync();
            return Result<List<string>>.Success(activeCards);
        }
        catch
        {
            return Result<List<string>>.Failure("Failed to retrieve active card numbers.");
        }
    }
    
    private async Task<Result<Membership>> ExtendMembershipAsync(int customerId, Func<DateTime, DateTime> extendDate)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var existingMembership = await _membershipRepository.GetLatestMembershipAsync(customerId);
            if (existingMembership is null)
            {
                return await RollbackWithFailure<Membership>("Membership does not exist. Nothing to extend.");
            }

            var existingCard = await _membershipRepository.GetActiveByCardNumber(existingMembership.CardNumber);
            if (existingCard?.IsActive is true && existingCard.CustomerId != customerId)
            {
                return await RollbackWithFailure<Membership>($"Card {existingCard.CardNumber} is already used by customerId {existingCard.CustomerId}.");
            }

            var baseDate = existingMembership is { IsActive: true, ExpiresAt: not null }
                ? existingMembership.ExpiresAt.Value
                : DateTime.UtcNow;

            existingMembership.ExpiresAt = extendDate(baseDate);

            var result = await _membershipRepository.UpdateAsync(existingMembership);
            return await CommitOrRollback(result);
        }
        catch
        {
            return await RollbackWithFailure<Membership>("Could not extend membership.");
        }
    }

    private async Task<Result> CommitOrRollback(Result result)
    {
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

    private async Task<Result<T>> CommitOrRollback<T>(Result<T> result)
    {
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

    private async Task<Result> RollbackWithFailure(string error)
    {
        await _uow.RollbackTransactionAsync();
        return Result.Failure(error);
    }

    private async Task<Result<T>> RollbackWithFailure<T>(string error)
    {
        await _uow.RollbackTransactionAsync();
        return Result<T>.Failure(error);
    }
}