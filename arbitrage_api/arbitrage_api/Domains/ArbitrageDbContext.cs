using arbitrage_api.Domains.CryptoArbitrages;
using arbitrage_api.Domains.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace arbitrage_api.Domains
{
    public class ArbitrageDbContext : IdentityDbContext<User>
    {
        public DbSet<CryptoArbitrage> CryptoArbitrages { get; set; }

        public ArbitrageDbContext(DbContextOptions<ArbitrageDbContext> options) : base(options) { }
    }
}
