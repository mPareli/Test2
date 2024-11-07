using System.Security.Claims;
using Emne_7___Arbeidskrav_2.Features.Users.DTO;


namespace Emne_7___Arbeidskrav_2.Features.Users.Interfaces;

public interface IUserService
{
    Task<ServiceResult<User>> RegisterUserAsync(UserRegistrationDTO userDto);
    Task<User?> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync(int page, int size);
    Task<ServiceResult> DeleteUserAsync(int id);
    Task<ServiceResult<User>> UpdateUserAsync(int id, UserUpdateDTO userDto);
    Task<bool> IsUserAuthorizedAsync(ClaimsPrincipal currentUser, int userId);
}