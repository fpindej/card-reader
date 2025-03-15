using CardReader.Application.Repositories;
using CardReader.Domain;
using CardReader.Infrastructure.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CardReader.Infrastructure.Persistence.Repositories;

internal class DeviceHealthRepository : IDeviceHealthRepository
{
    private readonly GymDoorDbContext _context;

    public DeviceHealthRepository(GymDoorDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(DeviceHealth deviceHealth)
    {
        await _context.DeviceHealths.AddAsync(deviceHealth);
    }

    public async Task<IEnumerable<DeviceHealth>> GetAllAsync(int pageNumber, int pageSize)
    {
        return await _context.DeviceHealths
            .OrderByDescending(deviceHealth => deviceHealth.CreatedAt)
            .Paginate(pageNumber, pageSize)
            .ToListAsync();
    }
}