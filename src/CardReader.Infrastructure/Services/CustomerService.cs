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

    public async Task<Result<Customer>> CreateUserAsync(string firstName, string lastName, string email)
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

            var existingEmail = await _customerRepository.GetByEmailAsync(user.Email);
            if (existingEmail is not null)
            {
                await _uow.RollbackTransactionAsync();
                return Result<Customer>.Failure("Email already exists");
            }
            
            var result = await _customerRepository.CreateCustomerAsync(user);
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
            return Result<Customer>.Failure("Failed to create customer.");
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

    public async Task<bool> DeleteByIdAsync(int id)
    {
        try
        {
            await _uow.BeginTransactionAsync();

            var deleted = await _customerRepository.DeleteAsync(id);
            if (!deleted)
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

    public async Task<Result<Customer>> GetByEmailAsync(string email)
    {
        var customer = await _customerRepository.GetByEmailAsync(email);

        if (customer is null)
        {
            return Result<Customer>.Failure("Customer with specified email is not found.");
        }
        
        return Result<Customer>.Success(customer);
    }
}