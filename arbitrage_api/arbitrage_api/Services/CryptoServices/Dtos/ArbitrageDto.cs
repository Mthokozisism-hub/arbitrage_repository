namespace arbitrage_api.Services.CryptoServices.Dtos
{
    public class ArbitrageDto
    {
        public decimal BitstampAskPrice { get; set; }
        public decimal ValrBidPrice { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal ArbitrageRatio { get; set; }
        public DateTime? Timestamp { get; set; }

    }
}
