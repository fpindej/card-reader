using CardReader.Domain;

namespace CardReader.Application.Services;

public interface IDeviceHealthService
{
    Task LogHealthAsync(int deviceId, int maxAllocHeap, int minFreeHeap, int freeHeap, int uptime, int freeSketchSpace);
    Task<IEnumerable<DeviceHealth>> GetAllLogsAsync(int pageNumber, int pageSize);
}
