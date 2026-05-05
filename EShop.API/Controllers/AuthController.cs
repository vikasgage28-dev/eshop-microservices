using Azure.Messaging.ServiceBus;
using EShop.Core.Interfaces;
using EShop.Shared.Common;
using EShop.Shared.DTOs;
using EShop.Shared.Messages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EShop.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ITokenService tokenService,
            ServiceBusClient serviceBusClient,
            ILogger<AuthController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _serviceBusClient = serviceBusClient;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterDto dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return BadRequest(ApiResponse<AuthResponseDto>.Fail("Email already registered"));

            var user = new IdentityUser { UserName = dto.Email, Email = dto.Email };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToList();
                return BadRequest(ApiResponse<AuthResponseDto>.Fail("Registration failed", errors));
            }

            await _userManager.AddToRoleAsync(user, "User");
            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user.Id, user.Email!, roles);

            // Publish welcome email message to Service Bus
            try
            {
                var sender = _serviceBusClient.CreateSender("welcome.email.queue");
                var message = new WelcomeEmailMessage
                {
                    Email = user.Email!,
                    UserName = $"{dto.FirstName} {dto.LastName}"
                };
                var sbMessage = new ServiceBusMessage(JsonSerializer.Serialize(message));
                await sender.SendMessageAsync(sbMessage);
                _logger.LogInformation("Welcome email message sent to Service Bus for {Email}", user.Email);
            }
            catch (Exception ex)
            {
                // Don't fail registration if Service Bus fails!
                _logger.LogError(ex, "Failed to send welcome email message for {Email}", user.Email);
            }

            return Ok(ApiResponse<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                FullName = $"{dto.FirstName} {dto.LastName}",
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            }, "Registration successful"));
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized(ApiResponse<AuthResponseDto>.Fail("Invalid email or password"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                return Unauthorized(ApiResponse<AuthResponseDto>.Fail("Invalid email or password"));

            var roles = await _userManager.GetRolesAsync(user);
            var token = _tokenService.CreateToken(user.Id, user.Email!, roles);

            return Ok(ApiResponse<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                FullName = user.UserName!,
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            }, "Login successful"));
        }
    }
}
