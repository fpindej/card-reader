using CardReader.Domain;

namespace CardReader.Application.Repositories;

public interface IAccessLogRepository
{
    Task CreateAsync(AccessLog accessLog);
    Task<List<AccessLog>> GetAllAsync(int pageNumber, int pageSize);
    Task CreateBatchAsync(IEnumerable<AccessLog> accessLogs);
}
