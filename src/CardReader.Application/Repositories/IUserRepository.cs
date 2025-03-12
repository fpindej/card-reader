using CardReader.Domain;

namespace CardReader.Application.Repositories;

public interface IUserRepository
{
    Task<int?> CreateUserAsync(User user);
}