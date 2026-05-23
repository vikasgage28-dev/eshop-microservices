using Identity.API.DTOs;
using Identity.Core.Features.Auth.Commands.Login;
using Identity.Core.Features.Auth.Commands.RefreshToken;
using Identity.Core.Features.Auth.Commands.Register;
using Identity.Core.Features.Auth.Queries.GetAllUsers;
using Identity.Core.Features.Auth.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            var result = await _mediator.Send(new RegisterCommand
            {
                FirstName = request.FirstName,
                LastName  = request.LastName,
                Email     = request.Email,
                Password  = request.Password,
                Role      = request.Role
            });

            if (!result.Success)
                return BadRequest(new { message = result.Error });

            return CreatedAtAction(nameof(GetMe), new AuthResponse
            {
                Token        = result.Token!,
                RefreshToken = result.RefreshToken!,
                UserId       = result.UserId,
                Email        = result.Email,
                FullName     = result.FullName
            });
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            var result = await _mediator.Send(new LoginCommand
            {
                Email    = request.Email,
                Password = request.Password
            });

            if (!result.Success)
                return Unauthorized(new { message = result.Error });

            return Ok(new AuthResponse
            {
                Token        = result.Token!,
                RefreshToken = result.RefreshToken!,
                UserId       = result.UserId,
                Email        = result.Email,
                FullName     = result.FullName,
                Roles        = result.Roles
            });
        }

        // POST api/auth/refresh
        [HttpPost("refresh")]
        public async Task<ActionResult<AuthResponse>> Refresh([FromBody] RefreshTokenRequest request)
        {
            var result = await _mediator.Send(new RefreshTokenCommand
            {
                RefreshToken = request.RefreshToken
            });

            if (!result.Success)
                return Unauthorized(new { message = result.Error });

            return Ok(new AuthResponse
            {
                Token        = result.Token!,
                RefreshToken = result.RefreshToken!
            });
        }

        // GET api/auth/me  — requires valid JWT
        [HttpGet("me")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                      ?? User.FindFirstValue("sub");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _mediator.Send(new GetUserByIdQuery(userId));
            if (user is null) return NotFound();

            return Ok(new UserDto
            {
                UserId    = user.Id,
                Email     = user.Email ?? string.Empty,
                FullName  = user.FullName,
                CreatedAt = user.CreatedAt
            });
        }

        // GET api/auth/users  — Admin only
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var users = await _mediator.Send(new GetAllUsersQuery());
            return Ok(users.Select(u => new UserDto
            {
                UserId    = u.Id,
                Email     = u.Email ?? string.Empty,
                FullName  = u.FullName,
                CreatedAt = u.CreatedAt
            }));
        }
    }
}
