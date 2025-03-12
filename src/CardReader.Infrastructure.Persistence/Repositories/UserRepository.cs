using CardReader.Application.Repositories;
using CardReader.Domain;
using Microsoft.EntityFrameworkCore;

namespace CardReader.Infrastructure.Persistence.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly GymDoorDbContext _context;

    public UserRepository(GymDoorDbContext context)
    {
        _context = context;
    }

    public async Task<int?> CreateUserAsync(User user)
    {
        try
        {
            var entry = await _context.Users.AddAsync(user);
            return entry.Entity.Id;
        }
        catch (DbUpdateException)
        {
            return null;
        }
    }
}