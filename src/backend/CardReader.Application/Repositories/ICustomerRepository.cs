using CardReader.Domain;

namespace CardReader.Application.Repositories;

public interface ICustomerRepository
{
    Task<Result<Customer>> CreateCustomerAsync(Customer customer);
    Task<Customer?> GetByIdAsync(int id);
    Task<List<Customer>> GetAllAsync(int pageNumber, int pageSize);
    Task<bool> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(int id);
    Task<Customer?> GetByEmailAsync(string email);
}