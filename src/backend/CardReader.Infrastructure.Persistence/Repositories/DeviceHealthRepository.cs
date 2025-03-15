using CardReader.Application.Repositories;
using CardReader.Domain;

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
}