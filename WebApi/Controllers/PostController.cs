using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Authorize]
[Route("posts")]
public class PostController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePostCommand command)
    {
        var id = await mediator.Send(command);
        return Ok(id);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id, [FromQuery] bool includeAuthor = false)
    {
        if(id < 1)
            return BadRequest("Post Id is invalid");
        
        var post = await mediator.Send(new GetPostByIdQuery(id, includeAuthor));
        
        if (post == null) 
            return NotFound();
        
        return Ok(post);
    }
}