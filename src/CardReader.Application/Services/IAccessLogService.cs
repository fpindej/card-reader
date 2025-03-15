using CardReader.Domain;

namespace CardReader.Application.Services;

public interface IAccessLogService
{
    Task LogAccessAsync(string cardNumber, bool isSuccess, DateTime timestamp);
    Task<List<AccessLog>> GetAllLogsAsync();
}
