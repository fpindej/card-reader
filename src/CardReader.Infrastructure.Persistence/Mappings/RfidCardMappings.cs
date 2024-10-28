using CardReader.Domain;

namespace CardReader.Infrastructure.Persistence.Mappings;

internal static class RfidCardMappings
{
    public static RfidCard ToDomain(this Models.RfidCard cardModel) => new()
    {
        Id = cardModel.Id,
        IsActive = cardModel.IsActive
    };

    public static Models.RfidCard ToModel(this RfidCard card) => new()
    {
        Id = card.Id,
        IsActive = card.IsActive
    };
}