using CardReader.Application.Repositories;
using CardReader.Domain;
using CardReader.Infrastructure.Persistence.Extensions;
using CardReader.Infrastructure.Persistence.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CardReader.Infrastructure.Persistence.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly GymDoorDbContext _context;

    public UserRepository(ILogger<UserRepository> logger, GymDoorDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<User> CreateAsync(User user)
    {
        var userModel = user.ToCreateModel();

        _ = await _context.Users.AddAsync(userModel);
        _ = await _context.SaveChangesAsync();
        _logger.LogInformation("Created new user with ID: {userId}", userModel.Id);
        
        return userModel.ToDomain();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var userModel = await _context.Users
            .Include(u => u.RfidCard)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        return userModel?.ToDomain();
    }

    public async Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize)
    {
        var usersModel = await _context.Users
            .Include(u => u.RfidCard)
            .OrderBy(u => u.Id)
            .Paginate(pageNumber, pageSize)
            .ToListAsync();

        return usersModel.Select(u => u.ToDomain());
    }

    public async Task<bool> UpdateAsync(User user)
    {
        var userModel = await _context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

        if (userModel is null)
            return false;

        userModel.FirstName = user.FirstName;
        userModel.LastName = user.LastName;
        userModel.YearOfBirth = user.YearOfBirth;

        _ = await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var userModel = await _context.Users.FindAsync(id);

        if (userModel is null)
            return false;
     
        _context.Users.Remove(userModel);
        _ = await _context.SaveChangesAsync();
        return true;
    }
}