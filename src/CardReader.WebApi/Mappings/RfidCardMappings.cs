using CardReader.Domain;
using CardReader.WebApi.Dtos;

namespace CardReader.WebApi.Mappings;

internal static class RfidCardMapping
{
    public static CreateRfidCardResponse ToResponse(this RfidCard rfidCard) 
        => new(rfidCard.Id, rfidCard.IsActive, rfidCard.UserId);

    public static RfidCard ToDomain(this CreateRfidCardRequest request) 
        => new()
        {
            Id = request.CardId,
            IsActive = true,
            UserId = request.UserId
        };
}