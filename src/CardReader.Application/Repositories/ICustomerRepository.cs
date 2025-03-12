using CardReader.Domain;

namespace CardReader.Application.Repositories;

public interface ICustomerRepository
{
    Task<int?> CreateCustomerAsync(Customer customer);
}