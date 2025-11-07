namespace GoProcure.Api.Dtos
{
    public sealed record RegisterDto(
        string Email,
        string Password
        //keep registration simple. After sigup, redirect to the page to update profile
        //string FirstName,
        //string LastName,
        //string Department
    );
}
