using Emne_7___Arbeidskrav_2.Features.Users.DTO;
using Emne_7___Arbeidskrav_2.Features.Users.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emne_7___Arbeidskrav_2.Features.Users;

[ApiController]
[Route("api/v1/users")]

public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userDTO)
    {
        var result = await _userService.RegisterUserAsync(userDTO);

        if (!result.Success)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetUserById), new { id = result.User.Id }, result.User);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllUsers([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var users = await _userService.GetAllUsersAsync(page, size);
        return Ok(users);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpGet("{id}/posts")]
    public async Task<IActionResult> GetUserPosts(int id)
    {
        var posts = await _userService.GetUserPostsAsync(id);
        if (posts == null)
            return NotFound();

        return Ok(posts);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var isAuthorized = await _userService.IsUserAuthorizedAsync(User, id);
        if (!isAuthorized)
            return Unauthorized();

        var result = await _userService.DeleteUserAsync(id);

        if (!result.Success)
            return BadRequest(result.Message);

        return NoContent();
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserUpdateDTO userDTO)
    {
        var isAuthorized = await _userService.IsUserAuthorizedAsync(User, id);
        if (!isAuthorized)
            return Unauthorized();

        var result = await _userService.UpdatedUserAsync(id, userDTO);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.User);
    }
}