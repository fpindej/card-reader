using CardReader.Application;
using CardReader.Application.Repositories;
using CardReader.Application.Services;
using CardReader.Domain;

namespace CardReader.Infrastructure.Services;

internal class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _uow;

    public CustomerService(ICustomerRepository customerRepository, IUnitOfWork uow)
    {
        _customerRepository = customerRepository;
        _uow = uow;
    }

    public async Task<int?> CreateUserAsync(string firstName, string lastName, string email)
    {
        var user = new Customer
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };

        await _uow.BeginTransactionAsync();

        try
        {
            var userId = await _customerRepository.CreateCustomerAsync(user);
            await _uow.CommitTransactionAsync();

            return userId;
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            return null;
        }
    }
}