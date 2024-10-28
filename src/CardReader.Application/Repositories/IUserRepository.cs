using CardReader.Domain;

namespace CardReader.Application.Repositories;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(int id);
}