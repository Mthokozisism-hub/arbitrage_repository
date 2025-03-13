namespace arbitrage_api.Services.CryptoServices
{
    public interface ICryptoService 
    {
        public Task<decimal> GetBitstampAskPrice(string cripto, string currency);
        public Task<decimal> GetValrBidPrice(string cripto, string currency);
        public Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency);
        public decimal CalculateArbitrage(decimal zarBid, decimal usdAsk, decimal exchangeRate);
    }
}
