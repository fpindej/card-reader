using CardReader.Application.Repositories;
using CardReader.Domain;
using Microsoft.EntityFrameworkCore;

namespace CardReader.Infrastructure.Persistence.Repositories;

internal class AccessLogRepository : IAccessLogRepository
{
    private readonly GymDoorDbContext _context;

    public AccessLogRepository(GymDoorDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(AccessLog accessLog)
    {
        await _context.AccessLogs.AddAsync(accessLog);
    }

    public async Task<List<AccessLog>> GetAllAsync()
    {
        return await _context.AccessLogs.ToListAsync();
    }
}
