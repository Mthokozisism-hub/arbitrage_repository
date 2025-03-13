using arbitrage_api.Domains.Users;

namespace arbitrage_api.Services.AuthTokenServices
{
    public interface IAuthTokenService
    {
        public string GenerateToken(User user);
    }
}
