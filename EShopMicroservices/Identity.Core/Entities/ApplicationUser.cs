namespace Identity.Core.Entities
{
    /// <summary>
    /// Pure domain entity — no dependency on ASP.NET Core Identity.
    /// Infrastructure maps AppIdentityUser ↔ ApplicationUser.
    /// </summary>
    public class ApplicationUser
    {
        public string  Id        { get; set; } = string.Empty;
        public string  UserName  { get; set; } = string.Empty;
        public string? Email     { get; set; }
        public string  FirstName { get; set; } = string.Empty;
        public string  LastName  { get; set; } = string.Empty;
        public string  FullName  => $"{FirstName} {LastName}".Trim();
        public DateTime  CreatedAt           { get; set; } = DateTime.UtcNow;
        public string?   RefreshToken        { get; set; }
        public DateTime? RefreshTokenExpiry  { get; set; }
        public bool      TwoFactorEnabled    { get; set; }
    }
}
