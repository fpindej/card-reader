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
        Notes = userModel.Notes,
        RfidCard = userModel.RfidCard?.ToDomain()
    };

    public static Models.User ToCreateModel(this User user) => new()
    {
        FirstName = user.FirstName,
        LastName = user.LastName,
        YearOfBirth = user.YearOfBirth,
        Notes = user.Notes,
        RfidCard = user.RfidCard?.ToModel()
    };
}