using CardReader.Application.Repositories;
using CardReader.Domain;
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
}