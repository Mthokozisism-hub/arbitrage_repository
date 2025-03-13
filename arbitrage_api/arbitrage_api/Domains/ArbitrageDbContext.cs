using arbitrage_api.Domains.CryptoArbitrages;
using Microsoft.EntityFrameworkCore;

namespace arbitrage_api.Domains
{
    public class ArbitrageDbContext : DbContext
    {
        public DbSet<CryptoArbitrage> CryptoArbitrages { get; set; }

        public ArbitrageDbContext(DbContextOptions<ArbitrageDbContext> options) : base(options) { }
    }
}
