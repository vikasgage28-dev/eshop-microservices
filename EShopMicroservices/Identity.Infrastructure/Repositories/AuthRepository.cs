using Identity.Core.Entities;
using Identity.Core.Interfaces;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<AppIdentityUser>   _userManager;
        private readonly SignInManager<AppIdentityUser>  _signInManager;
        private readonly AppIdentityDbContext            _context;

        public AuthRepository(
            UserManager<AppIdentityUser>   userManager,
            SignInManager<AppIdentityUser>  signInManager,
            AppIdentityDbContext            context)
        {
            _userManager   = userManager;
            _signInManager = signInManager;
            _context       = context;
        }

        // ── Mapping helper ────────────────────────────────────────────────
        private static ApplicationUser ToModel(AppIdentityUser u) => new()
        {
            Id                 = u.Id,
            UserName           = u.UserName ?? string.Empty,
            Email              = u.Email,
            FirstName          = u.FirstName,
            LastName           = u.LastName,
            CreatedAt          = u.CreatedAt,
            RefreshToken       = u.RefreshToken,
            RefreshTokenExpiry = u.RefreshTokenExpiry
        };

        // ── IAuthRepository ───────────────────────────────────────────────
        public async Task<(bool Success, string? Error)> RegisterAsync(
            ApplicationUser user, string password, string role)
        {
            var identityUser = new AppIdentityUser
            {
                UserName  = user.Email ?? user.UserName,
                Email     = user.Email,
                FirstName = user.FirstName,
                LastName  = user.LastName,
                CreatedAt = user.CreatedAt,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(identityUser, password);
            if (!result.Succeeded)
                return (false, string.Join("; ", result.Errors.Select(e => e.Description)));

            await _userManager.AddToRoleAsync(identityUser, role);

            // Propagate generated Id back to the domain model
            user.Id = identityUser.Id;
            return (true, null);
        }

        public async Task<ApplicationUser?> ValidateCredentialsAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return null;

            var result = await _signInManager.CheckPasswordSignInAsync(
                user, password, lockoutOnFailure: false);
            return result.Succeeded ? ToModel(user) : null;
        }

        public async Task<ApplicationUser?> GetByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user is null ? null : ToModel(user);
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is null ? null : ToModel(user);
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            var users = await _context.Users.OrderBy(u => u.LastName).ToListAsync();
            return users.Select(ToModel);
        }

        public async Task UpdateRefreshTokenAsync(string userId, string refreshToken, DateTime expiry)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) return;

            user.RefreshToken       = refreshToken;
            user.RefreshTokenExpiry = expiry;
            await _userManager.UpdateAsync(user);
        }

        public async Task<ApplicationUser?> GetByRefreshTokenAsync(string refreshToken)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            return user is null ? null : ToModel(user);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            var identityUser = await _userManager.FindByIdAsync(user.Id);
            return identityUser is null
                ? new List<string>()
                : await _userManager.GetRolesAsync(identityUser);
        }
    }
}
