using Emne_7___Arbeidskrav_2.Features.Posts.DTO;
using Emne_7___Arbeidskrav_2.Features.Posts.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emne_7___Arbeidskrav_2.Features.Posts;

[ApiController]
[Route("api/v1/posts")]
public class PostController : ControllerBase
{
    private readonly IPostService _postService;

    public PostController(IPostService postService)
    {
        _postService = postService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllPosts([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var posts = await _postService.GetAllPostsAsync(page, size);
        return Ok(posts);
    }

    [Authorize]
    [HttpGet("{postId}")]
    public async Task<IActionResult> GetPostById(int postId)
    {
        var post = await _postService.GetPostByIdAsync(postId);
        
        if (post == null)
            return NotFound();
        
        return Ok(post);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] PostCreateDTO postDto)
    {
        var result = await _postService.CreatePostAsync(postDto, User);
        
        if (!result.Success)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetPostById), new { postId = result.Post.Id }, result.Post);
    }

    [Authorize]
    [HttpPut("{postId}")]
    public async Task<IActionResult> UpdatePost(int postId, [FromBody] PostUpdateDTO postDto)
    {
        var isAuthorized = await _postService.IsUserAuthorizedAsync(User, postId);
        if (!isAuthorized)
            return Unauthorized();

        var result = await _postService.UpdatePostAsync(postId, postDto);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Post);
    }

    [Authorize]
    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeletePost(int postId)
    {
        var isAuthorized = await _postService.IsUserAuthorizedAsync(User, postId);
        if (!isAuthorized)
            return Unauthorized();

        var result = await _postService.DeletePostAsync(postId);

        if (!result.Success)
            return BadRequest(result.Message);

        return NoContent();
    }
}