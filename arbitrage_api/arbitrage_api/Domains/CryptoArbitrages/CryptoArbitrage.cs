namespace arbitrage_api.Domains.CryptoArbitrages
{
    public class CryptoArbitrage
    {
        public int Id { get; set; }
        public string CryptoName { get; set; }
        public decimal BitstampAskPrice { get; set; }
        public decimal ValrBidPrice { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal ArbitrageRatio { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
