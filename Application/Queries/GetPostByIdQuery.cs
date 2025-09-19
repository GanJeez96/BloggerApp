using Application.Dtos;
using MediatR;

namespace Application.Queries;

public record GetPostByIdQuery(long PostId, bool IncludeAuthor = false) : IRequest<PostDto?>;