﻿using CardReader.Application;
using CardReader.Application.Repositories;
using CardReader.Domain;
using Microsoft.Extensions.Logging;

namespace CardReader.Infrastructure;

public class UserService : IUserService
{
    private readonly ILogger<UserService> _logger;
    private readonly IUserRepository _userRepository;

    public UserService(ILogger<UserService> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task CreateAsync(User user)
    {
        _logger.LogInformation("Creating user...");

        await _userRepository.CreateAsync(user);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting user...");

        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync(int pageNumber, int pageSize)
    {
        _logger.LogInformation("Getting all users...");
        
        return await _userRepository.GetAllAsync(pageNumber, pageSize);
    }

    public async Task<bool> UpdateAsync(User user)
    {
        _logger.LogInformation("Updating user...");

        return await _userRepository.UpdateAsync(user);
    }

    public async Task<bool> DeleteByIdAsync(Guid id)
    {
        _logger.LogInformation("Deleting user...");

        return await _userRepository.DeleteAsync(id);
    }
}