namespace EShop.Core.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(string userId, string email, IList<string> roles);
    }
}
