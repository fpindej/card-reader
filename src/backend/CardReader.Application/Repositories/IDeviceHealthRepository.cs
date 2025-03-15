using CardReader.Domain;

namespace CardReader.Application.Repositories;


public interface IDeviceHealthRepository
{
    Task CreateAsync(DeviceHealth deviceHealth);
    Task<IEnumerable<DeviceHealth>> GetAllAsync(int pageNumber, int pageSize);
}