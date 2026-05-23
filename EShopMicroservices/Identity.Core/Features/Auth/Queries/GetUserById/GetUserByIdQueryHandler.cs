using Identity.Core.Entities;
using Identity.Core.Interfaces;
using MediatR;

namespace Identity.Core.Features.Auth.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApplicationUser?>
    {
        private readonly IAuthRepository _authRepository;

        public GetUserByIdQueryHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<ApplicationUser?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
            => await _authRepository.GetByIdAsync(request.UserId);
    }
}
