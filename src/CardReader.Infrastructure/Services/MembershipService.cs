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

    public async Task<bool> CreateMembershipAsync(int customerId, string cardNumber, DateTime? expiresAt = null)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer is null)
            {
                await _uow.RollbackTransactionAsync();
                return false;
            }

            var membership = new Membership
            {
                CustomerId = customerId,
                CardNumber = cardNumber,
                ExpiresAt = expiresAt
            };

            var created = await _membershipRepository.CreateAsync(membership);
            if (!created)
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

    public async Task<bool> ValidateCardAccessAsync(string cardNumber)
    {
        var card = await _membershipRepository.GetCardByNumberAsync(cardNumber);
        return card?.IsActive ?? false;
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
}