namespace CardReader.Application.Services;

public interface IMembershipService
{
    Task<bool> CreateMembershipAsync(int customerId, string cardNumber, DateTime? expiresAt = null);
    Task<bool> ValidateCardAccessAsync(string cardNumber);
}