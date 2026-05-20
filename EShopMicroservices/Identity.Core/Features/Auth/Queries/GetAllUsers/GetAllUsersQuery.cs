using Identity.Core.Entities;
using MediatR;

namespace Identity.Core.Features.Auth.Queries.GetAllUsers
{
    public record GetAllUsersQuery : IRequest<IEnumerable<ApplicationUser>>;
}
