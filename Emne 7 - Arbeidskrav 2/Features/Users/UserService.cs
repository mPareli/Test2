using System.Security.Claims;
using Emne_7___Arbeidskrav_2.Features.Users.DTO;
using Emne_7___Arbeidskrav_2.Features.Users.Interfaces;

namespace Emne_7___Arbeidskrav_2.Features.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ServiceResult<User>> RegisterUserAsync(UserRegistrationDTO userDto)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        var user = new User
        {
            UserName = userDto.UserName,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            HashedPassword = hashedPassword,
            IsAdminUser = userDto.IsAdminUser
        };

        var createdUser = await _userRepository.RegisterUserAsync(user);
        return new ServiceResult<User>(createdUser);
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync(int page, int size)
    {
        return await _userRepository.GetAllUsersAsync(page, size);
    }

    public async Task<ServiceResult> DeleteUserAsync(int id)
    {
        var success = await _userRepository.DeleteUserAsync(id);
        return success ? ServiceResult.Success() : ServiceResult.Failure("User deletion failed.");
    }

    public async Task<ServiceResult<User>> UpdateUserAsync(int id, UserUpdateDTO userDto)
    {
        var existingUser = await _userRepository.GetUserByIdAsync(id);
        if (existingUser == null)
            return ServiceResult<User>.Failure("User not found.");

        existingUser.UserName = userDto.UserName ?? existingUser.UserName;
        existingUser.FirstName = userDto.FirstName ?? existingUser.FirstName;
        existingUser.LastName = userDto.LastName ?? existingUser.LastName;
        existingUser.Email = userDto.Email ?? existingUser.Email;
        if (!string.IsNullOrEmpty(userDto.Password))
            existingUser.HashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

        var updatedUser = await _userRepository.UpdateUserAsync(existingUser);
        return updatedUser != null ? ServiceResult<User>.Success(updatedUser) : ServiceResult<User>.Failure("Update failed.");
    }

    public async Task<bool> IsUserAuthorizedAsync(ClaimsPrincipal currentUser, int userId)
    {
        var currentUserId = int.Parse(currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        return await _userRepository.IsUserAuthorizedAsync(userId, currentUserId);
    }
}