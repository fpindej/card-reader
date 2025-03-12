namespace CardReader.Application;

public interface IMembershipService
{
    Task<bool> ActivateAsync(int membershipId);
    Task<bool> DeactivateAsync(int membershipId);
    Task<bool> ExtendAsync(int membershipId, int days);
    Task<bool> RevokeAsync(int membershipId);
}
