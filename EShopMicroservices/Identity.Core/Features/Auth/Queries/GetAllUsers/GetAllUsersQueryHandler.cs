using Identity.Core.Entities;
using Identity.Core.Interfaces;
using MediatR;

namespace Identity.Core.Features.Auth.Queries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<ApplicationUser>>
    {
        private readonly IAuthRepository _authRepository;

        public GetAllUsersQueryHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<IEnumerable<ApplicationUser>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
            => await _authRepository.GetAllAsync();
    }
}
