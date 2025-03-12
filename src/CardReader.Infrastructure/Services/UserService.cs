using CardReader.Application;
using CardReader.Application.Repositories;
using CardReader.Application.Services;
using CardReader.Domain;

namespace CardReader.Infrastructure.Services;

internal class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _uow;

    public UserService(IUserRepository userRepository, IUnitOfWork uow)
    {
        _userRepository = userRepository;
        _uow = uow;
    }

    public async Task<int?> CreateUserAsync(string firstName, string lastName, string email)
    {
        var user = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Email = email
        };

        await _uow.BeginTransactionAsync();

        try
        {
            var userId = await _userRepository.CreateUserAsync(user);
            await _uow.CommitTransactionAsync();

            return userId;
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            return null;
        }
    }
}