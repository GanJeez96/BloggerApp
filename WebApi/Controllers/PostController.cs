using Application.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
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
    public async Task<IActionResult> GetById(long id)
    {
        var post = await mediator.Send(new GetPostByIdQuery(id));
        
        if (post == null) 
            return NotFound();
        
        return Ok(post);
    }
}