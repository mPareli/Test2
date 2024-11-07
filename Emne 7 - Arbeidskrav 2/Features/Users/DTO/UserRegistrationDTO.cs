namespace Emne_7___Arbeidskrav_2.Features.Users.DTO;

public class UserRegistrationDTO
{
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsAdminUser { get; set; } = false;
}