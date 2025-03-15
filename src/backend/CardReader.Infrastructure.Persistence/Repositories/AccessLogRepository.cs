using CardReader.Application.Repositories;
using CardReader.Domain;
using CardReader.Infrastructure.Persistence.Extensions;
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

    public async Task<List<AccessLog>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.AccessLogs
            .OrderByDescending(x => x.EventDateTime)
            .Paginate(pageNumber, pageSize)
            .ToListAsync();
    }

    public Task CreateBatchAsync(IEnumerable<AccessLog> accessLogs)
    {
        return _context.AccessLogs.AddRangeAsync(accessLogs);
    }
}
