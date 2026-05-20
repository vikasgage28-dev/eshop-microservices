using Identity.Core.Entities;
using MediatR;

namespace Identity.Core.Features.Auth.Queries.GetUserById
{
    public record GetUserByIdQuery(string UserId) : IRequest<ApplicationUser?>;
}
