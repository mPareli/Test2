namespace Emne_7___Arbeidskrav_2.Features.Users.DTO;

public class UserRequestDTO
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsAdminUser { get; set; } = false;
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
}