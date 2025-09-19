using Domain.Entities;
using MediatR;

namespace Application.Queries;

public record GetPostByIdQuery(long PostId) : IRequest<Post?>;