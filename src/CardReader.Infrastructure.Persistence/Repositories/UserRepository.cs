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

    public async Task CreateAsync(User user)
    {
        var userModel = user.ToModel();

        _ = await _context.Users.AddAsync(userModel);
        var entriesCount = await _context.SaveChangesAsync();
        _logger.LogInformation("Saved entries count: {entriesCount}", entriesCount.ToString());
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        var userModel = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        
        return userModel?.ToDomain();
    }

    public async Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize)
    {
        var usersModel = await _context.Users
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

    public async Task<bool> DeleteAsync(Guid id)
    {
        var userModel = await _context.Users.FindAsync(id);

        if (userModel is null)
            return false;
     
        _context.Users.Remove(userModel);
        _ = await _context.SaveChangesAsync();
        return true;
    }
}