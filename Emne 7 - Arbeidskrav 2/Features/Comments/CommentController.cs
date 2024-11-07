using Emne_7___Arbeidskrav_2.Features.Comments.DTO;
using Emne_7___Arbeidskrav_2.Features.Comments.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Emne_7___Arbeidskrav_2.Features.Comments;

[ApiController]
[Route("api/v1/comments")]
public class CommentController : ControllerBase
{
    private readonly ICommentService _commentService;

    public CommentController(ICommentService commentService)
    {
        _commentService = commentService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAllComments([FromQuery] int page = 1, [FromQuery] int size = 10)
    {
        var comments = await _commentService.GetAllCommentsAsync(page, size);
        return Ok(comments);
    }

    [Authorize]
    [HttpGet("{commentId}")]
    public async Task<IActionResult> GetCommentById(int commentId)
    {
        var comment = await _commentService.GetCommentByIdAsync(commentId);
        
        if (comment == null)
            return NotFound();
        
        return Ok(comment);
    }

    [HttpGet("{postId}/comments")]
    public async Task<IActionResult> GetCommentsByPostId(int postId)
    {
        var comments = await _commentService.GetCommentsByPostIdAsync(postId);

        if (comments == null || !comments.Any())
            return NotFound();

        return Ok(comments);
    }

    [Authorize]
    [HttpPost("{postId}")]
    public async Task<IActionResult> CreateComment(int postId, [FromBody] CommentCreateDTO commentDto)
    {
        var result = await _commentService.CreateCommentAsync(postId, commentDto, User);
        
        if (!result.Success)
            return BadRequest(result.Message);

        return CreatedAtAction(nameof(GetCommentById), new { commentId = result.Comment.Id }, result.Comment);
    }

    [Authorize]
    [HttpPut("{commentId}")]
    public async Task<IActionResult> UpdateComment(int commentId, [FromBody] CommentUpdateDTO commentDto)
    {
        var isAuthorized = await _commentService.IsUserAuthorizedAsync(User, commentId);
        if (!isAuthorized)
            return Unauthorized();

        var result = await _commentService.UpdateCommentAsync(commentId, commentDto);

        if (!result.Success)
            return BadRequest(result.Message);

        return Ok(result.Comment);
    }

    [Authorize]
    [HttpDelete("{commentId}")]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var isAuthorized = await _commentService.IsUserAuthorizedAsync(User, commentId);
        if (!isAuthorized)
            return Unauthorized();

        var result = await _commentService.DeleteCommentAsync(commentId);

        if (!result.Success)
            return BadRequest(result.Message);

        return NoContent();
    }
}