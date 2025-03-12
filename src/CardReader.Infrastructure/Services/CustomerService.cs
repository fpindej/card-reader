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

        try
        {
            await _uow.BeginTransactionAsync();
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

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<List<Customer>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _customerRepository.GetAllAsync(pageNumber, pageSize);
    }
    
    public async Task<bool> UpdateAsync(int id, string? firstName, string? lastName, string? email)
    {
        var customer = new Customer
        {
            Id = id,
            FirstName = firstName!,
            LastName = lastName!,
            Email = email!
        };

        try
        {
            await _uow.BeginTransactionAsync();
        
            var updated = await _customerRepository.UpdateAsync(customer);

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