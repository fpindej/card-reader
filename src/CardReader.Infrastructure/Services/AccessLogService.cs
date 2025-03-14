using CardReader.Application;
using CardReader.Application.Repositories;
using CardReader.Application.Services;
using CardReader.Domain;

namespace CardReader.Infrastructure.Services;

internal class AccessLogService : IAccessLogService
{
    private readonly IAccessLogRepository _accessLogRepository;
    private readonly IUnitOfWork _uow;

    public AccessLogService(IAccessLogRepository accessLogRepository, IUnitOfWork uow)
    {
        _accessLogRepository = accessLogRepository;
        _uow = uow;
    }

    public async Task LogAccessAsync(string cardNumber, bool isSuccess)
    {
        var accessLog = new AccessLog
        {
            EventDateTime = DateTime.UtcNow,
            CardNumber = cardNumber,
            IsSuccessful = isSuccess
        };

        try
        {
            await _uow.BeginTransactionAsync();
            await _accessLogRepository.CreateAsync(accessLog);
            await _uow.CommitTransactionAsync();
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<List<AccessLog>> GetAllLogsAsync()
    {
        return await _accessLogRepository.GetAllAsync();
    }
}
