using CardReader.Domain;

namespace CardReader.Application.Services;

public interface ICustomerService
{
    Task<int?> CreateUserAsync(string firstName, string lastName, string email);
}