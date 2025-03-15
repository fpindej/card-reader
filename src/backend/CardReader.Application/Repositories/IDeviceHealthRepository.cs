using CardReader.Domain;

namespace CardReader.Application.Repositories;


public interface IDeviceHealthRepository
{
    Task CreateAsync(DeviceHealth deviceHealth);
}