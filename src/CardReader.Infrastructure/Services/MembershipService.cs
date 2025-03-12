using CardReader.Application;
using CardReader.Application.Repositories;
using CardReader.Application.Services;
using CardReader.Domain;

namespace CardReader.Infrastructure.Services;

public class MembershipService : IMembershipService
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
            
            // ToDo: Check if card number is already in use

            var membership = new Membership
            {
                CustomerId = customerId,
                AccessCard = new AccessCard { CardNumber = cardNumber },
                ExpiresAt = expiresAt,
                IsActive = true
            };

            var created = await _membershipRepository.CreateAsync(membership);

            if (created)
            {
                await _uow.CommitTransactionAsync();
                return true;
            }
            
            await _uow.RollbackTransactionAsync();
            return false;
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
        return card?.HasValidMembership() ?? false;
    }
}