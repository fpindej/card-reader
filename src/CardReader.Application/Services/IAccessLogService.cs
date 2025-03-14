using CardReader.Domain;

namespace CardReader.Application.Services;

public interface IAccessLogService
{
    Task LogAccessAsync(string cardNumber, bool isSuccess);
    Task<List<AccessLog>> GetAllLogsAsync();
}
