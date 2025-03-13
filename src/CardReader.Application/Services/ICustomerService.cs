using CardReader.Domain;

namespace CardReader.Application.Services;

public interface ICustomerService
{
    Task<Result<Customer>> CreateUserAsync(string firstName, string lastName, string email);
    Task<Customer?> GetByIdAsync(int id);
    Task<List<Customer>> GetAllAsync(int pageNumber, int pageSize);
    Task<bool> UpdateAsync(int id, string? firstName, string? lastName, string? email);
    Task<bool> DeleteByIdAsync(int id);
    Task<Result<Customer>> GetByEmailAsync(string email);
}