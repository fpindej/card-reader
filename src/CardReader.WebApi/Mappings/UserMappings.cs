﻿using CardReader.Domain;
using CardReader.WebApi.Dtos;

namespace CardReader.WebApi.Mappings;

internal static class UserMappings
{
    public static User ToDomain(this CreateUserRequest request, RfidCard? card = null) => new()
    {
        FirstName = request.FirstName,
        LastName = request.LastName,
        YearOfBirth = request.YearOfBirth,
        Notes = request.Notes,
        RfidCard = card
    };

    public static CreateUserResponse ToResponse(this User user) 
        => new(user.Id, user.FirstName, user.LastName, user.YearOfBirth, user.Notes);

    public static User ToDomain(this UpdateUserRequest request) => new()
    {
        Id = request.Id,
        FirstName = request.FirstName,
        LastName = request.LastName,
        YearOfBirth = request.YearOfBirth,
        Notes = request.Notes
    };
}