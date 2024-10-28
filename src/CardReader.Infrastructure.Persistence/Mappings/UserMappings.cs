using CardReader.Domain;

namespace CardReader.Infrastructure.Persistence.Mappings;

internal static class UserMapping
{
    public static User ToDomain(this Models.User userModel) => new()
    {
        Id = userModel.Id,
        FirstName = userModel.FirstName,
        LastName = userModel.LastName,
        YearOfBirth = userModel.YearOfBirth,
        RfidId = userModel.RfidId
    };

    public static Models.User ToCreateModel(this User user) => new()
    {
        FirstName = user.FirstName,
        LastName = user.LastName,
        YearOfBirth = user.YearOfBirth,
        RfidId = user.RfidId
    };
}