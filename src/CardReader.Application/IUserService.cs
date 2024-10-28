using CardReader.Domain;

namespace CardReader.Application;

public interface IUserService
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByRfidId(string id);
    Task<List<User>> GetValidUsersAsync();
    Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteByIdAsync(int id);
}