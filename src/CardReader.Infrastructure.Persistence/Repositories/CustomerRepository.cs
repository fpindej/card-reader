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

    public async Task<int?> CreateCustomerAsync(Customer customer)
    {
        try
        {
            var entry = await _context.Customers.AddAsync(customer);
            return entry.Entity.Id;
        }
        catch (DbUpdateException)
        {
            return null;
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
}