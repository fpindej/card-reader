using CardReader.Application;
using CardReader.Application.Repositories;
using CardReader.Application.Services;
using CardReader.Domain;

namespace CardReader.Infrastructure.Services;

internal class DeviceHealthService : IDeviceHealthService
{
    private readonly IDeviceHealthRepository _deviceHealthRepository;
    private readonly IUnitOfWork _uow;

    public DeviceHealthService(IDeviceHealthRepository deviceHealthRepository, IUnitOfWork uow)
    {
        _deviceHealthRepository = deviceHealthRepository;
        _uow = uow;
    }

    public async Task LogHealthAsync(int deviceId, int maxAllocHeap, int minFreeHeap, int freeHeap, int uptime,
        int freeSketchSpace)
    {
        var deviceHealth = new DeviceHealth
        {
            DeviceId = deviceId,
            MaxAllocHeap = maxAllocHeap,
            MinFreeHeap = minFreeHeap,
            FreeHeap = freeHeap,
            Uptime = uptime,
            FreeSketchSpace = freeSketchSpace
        };

        try
        {
            await _uow.BeginTransactionAsync();
            await _deviceHealthRepository.CreateAsync(deviceHealth);
            await _uow.CommitTransactionAsync();
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}
