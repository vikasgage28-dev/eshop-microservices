using MediatR;

namespace Identity.Core.Features.Auth.Commands.Register
{
    public class RegisterCommand : IRequest<RegisterResult>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName  { get; set; } = string.Empty;
        public string Email     { get; set; } = string.Empty;
        public string Password  { get; set; } = string.Empty;
        public string Role      { get; set; } = "Customer";
    }

    public class RegisterResult
    {
        public bool   Success      { get; init; }
        public string? Error       { get; init; }
        public string? Token       { get; init; }
        public string? RefreshToken{ get; init; }
        public string? UserId      { get; init; }
        public string? Email       { get; init; }
        public string? FullName    { get; init; }
    }
}
