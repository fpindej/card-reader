using CardReader.Domain;
using CardReader.WebApi.Dtos;

namespace CardReader.WebApi.Mappings;

public static class MembershipMappings
{
    public static Membership ToDomain(this CreateMembershipRequest request) => new()
    {
        UserId = request.UserId,
        StartDate = request.StartDate,
        EndDate = request.EndDate,
        IsActive = false
    };

    public static CreateMembershipResponse ToResponse(this Membership membership) => new(
        membership.Id,
        membership.UserId,
        membership.StartDate,
        membership.EndDate,
        membership.IsActive
    );

    public static MembershipResponse ToMembershipResponse(this Membership membership) => new(
        membership.Id,
        membership.UserId,
        membership.StartDate,
        membership.EndDate,
        membership.IsActive
    );
}
