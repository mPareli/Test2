using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Emne_7___Arbeidskrav_2.Features.Users.DTO;
using Emne_7___Arbeidskrav_2.Features.Users.Interfaces;


namespace Emne_7___Arbeidskrav_2.Features.Users.Interfaces;

public class UserRepository : IUserRepository
{
    private readonly DbContext _context;

    public UserRepository(DbContext context)
    {
        _context = context;
    }
    
    public async Task<User> RegisterUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
    
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Posts)   
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    public async Task<IEnumerable<User>> GetAllUsersAsync(int page, int size)
    {
        return await _context.Users
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync();
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> UpdateUserAsync(User updatedUser)
    {
        var user = await _context.Users.FindAsync(updatedUser.Id);
        if (user == null) return null;

        user.UserName = updatedUser.UserName;
        user.FirstName = updatedUser.FirstName;
        user.LastName = updatedUser.LastName;
        user.Email = updatedUser.Email;
        user.HashedPassword = updatedUser.HashedPassword;
        user.IsAdminUser = updatedUser.IsAdminUser;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> IsUserAuthorizedAsync(int userId, int currentUserId)
    {
       
        var user = await _context.Users.FindAsync(userId);
        return user != null && user.Id == currentUserId;
    }
}