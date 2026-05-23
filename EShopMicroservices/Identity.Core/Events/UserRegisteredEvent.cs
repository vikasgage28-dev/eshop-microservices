namespace Identity.Core.Events
{
    public class UserRegisteredEvent
    {
        public string UserId    { get; init; } = string.Empty;
        public string Email     { get; init; } = string.Empty;
        public string FullName  { get; init; } = string.Empty;
        public string Role      { get; init; } = string.Empty;
        public DateTime OccurredAt { get; init; } = DateTime.UtcNow;
    }
}
