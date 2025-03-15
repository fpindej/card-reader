using CardReader.Application.Repositories;
using CardReader.Domain;
using CardReader.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CardReader.Infrastructure.Persistence.Repositories;

internal class CustomerRepository : ICustomerRepository
{
    private readonly GymDoorDbContext _context;

    public CustomerRepository(GymDoorDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Customer>> CreateCustomerAsync(Customer customer)
    {
        try
        {
            await _context.Customers.AddAsync(customer);
            return Result<Customer>.Success(customer);
        }
        catch
        {
            return Result<Customer>.Failure("Failed to create customer.");
        }
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task<List<Customer>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.Customers
            .OrderBy(u => u.Id)
            .Paginate(pageNumber, pageSize)
            .ToListAsync();
    }
    
    public async Task<bool> UpdateAsync(Customer customer)
    {
        var customerinDb = await _context.Customers
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == customer.Id);

        if (customerinDb is null)
        {
            return false;
        }

        if (!string.IsNullOrWhiteSpace(customer.FirstName))
        {
            customerinDb.FirstName = customer.FirstName;
        }
        
        if (!string.IsNullOrWhiteSpace(customer.LastName))
        {
            customerinDb.LastName = customer.LastName;
        }
        
        if (!string.IsNullOrWhiteSpace(customer.Email))
        {
            customerinDb.Email = customer.Email;
        }

        return true;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        var customer = await _context.Customers
            .AsTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (customer is null)
        {
            return false;
        }

        _context.Customers.Remove(customer);
        return true;
    }

    public Task<Customer?> GetByEmailAsync(string email)
    {
        var customer = _context.Customers
            .AsNoTracking()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync();

        return customer;
    }
}