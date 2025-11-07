namespace GoProcure.Api.Dtos
{
    public sealed record UpdateMyProfileDto(
    string FirstName,
    string LastName,
    string Department,
    string? JobTitle,
    DateTime? DateOfBirth
);
}
