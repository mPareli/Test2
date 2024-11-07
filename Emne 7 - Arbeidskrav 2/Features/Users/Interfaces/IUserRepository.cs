namespace Emne_7___Arbeidskrav_2.Features.Users.Interfaces;

public interface IUserRepository
{
    Task<User> RegisterUserAsync(User user);
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync(int page, int size);
    Task<bool> DeleteUserAsync(int id);
    Task<User?> UpdateUserAsync(User updatedUser);
    Task<bool> IsUserAuthorizedAsync(int userId, int currentUserId);
}