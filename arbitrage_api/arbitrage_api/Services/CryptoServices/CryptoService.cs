using arbitrage_api.Domains;
using arbitrage_api.Domains.CryptoArbitrages;
using arbitrage_api.Services.CryptoServices.Dtos;
using arbitrage_api.Utils;
using Microsoft.Extensions.Options;

namespace arbitrage_api.Services.CryptoServices
{
    /// <summary>
    /// Service class responsible for interacting with cryptocurrency-related APIs
    /// to fetch prices, exchange rates, and calculate arbitrage opportunities.
    /// </summary>
    public class CryptoService : ICryptoService
    {
        // HttpClient instance for making HTTP requests to external APIs.
        private readonly HttpClient _httpClient;

        private readonly ArbitrageDbContext _context;

        // Configuration settings for arbitrage, such as API keys and other parameters.
        private readonly ArbitrageSettings _arbitrageSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoService"/> class.
        /// </summary>
        /// <param name="httpClient">The HttpClient instance for making API requests.</param>
        /// <param name="arbitrageSettings">The configuration settings for arbitrage.</param>
        public CryptoService(HttpClient httpClient, ArbitrageDbContext context, IOptions<ArbitrageSettings> arbitrageSettings)
        {
            _httpClient = httpClient;
            _arbitrageSettings = arbitrageSettings.Value;
            _context = context;
        }

        /// <summary>
        /// Fetches the ask price for a specified cryptocurrency and currency pair from Bitstamp.
        /// </summary>
        /// <param name="cripto">The cryptocurrency symbol (e.g., "btc").</param>
        /// <param name="currency">The currency symbol (e.g., "usd").</param>
        /// <returns>The ask price as a decimal value.</returns>
        public async Task<decimal> GetBitstampAskPrice(string cripto, string currency)
        {
            var url = $"https://www.bitstamp.net/api/v2/ticker/{cripto}{currency}";
            var response = await _httpClient.GetFromJsonAsync<BitstampResponseDto>(url);
            return response.Ask;
        }

        /// <summary>
        /// Fetches the bid price for a specified cryptocurrency and currency pair from VALR.
        /// </summary>
        /// <param name="cripto">The cryptocurrency symbol (e.g., "btc").</param>
        /// <param name="currency">The currency symbol (e.g., "zar").</param>
        /// <returns>The bid price as a decimal value.</returns>
        public async Task<decimal> GetValrBidPrice(string cripto, string currency)
        {
            var url = $"https://api.valr.com/v1/public/{cripto}{currency}/marketsummary";
            var response = await _httpClient.GetFromJsonAsync<ValrResponseDto>(url);
            return decimal.Parse(response.BidPrice);
        }

        /// <summary>
        /// Fetches the exchange rate between two currencies using an external API.
        /// </summary>
        /// <param name="fromCurrency">The source currency symbol (e.g., "usd").</param>
        /// <param name="toCurrency">The target currency symbol (e.g., "zar").</param>
        /// <returns>The exchange rate as a decimal value.</returns>
        public async Task<decimal> GetExchangeRate(string fromCurrency, string toCurrency)
        {
            var url = $"https://v6.exchangerate-api.com/v6/{_arbitrageSettings.ExchangeRateApiKey}/pair/{fromCurrency}/{toCurrency}";

            var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponseDto>(url);
            return response.Conversion_Rate;
        }

        /// <summary>
        /// Calculates the arbitrage opportunity between two prices and an exchange rate.
        /// </summary>
        /// <param name="zarBid">The bid price in ZAR (South African Rand).</param>
        /// <param name="usdAsk">The ask price in USD (US Dollar).</param>
        /// <param name="exchangeRate">The exchange rate between USD and ZAR.</param>
        /// <returns>The arbitrage value as a decimal.</returns>
        public decimal CalculateArbitrage(decimal zarBid, decimal usdAsk, decimal exchangeRate)
        {
            return zarBid / (usdAsk * exchangeRate);
        }

        /// <summary>
        /// Calculates the arbitrage opportunity between two prices and an exchange rate.
        /// </summary>
        /// <param name="criptoName">The Cripto name for the arbitrade data requested for .</param>
        /// <param name="bidCurrency">The Bid price Currency </param>
        /// <param name="AskCurrency">The Ask price Currency</param>
        /// <returns>The arbitrage value as a decimal.</returns>
        public async Task<ArbitrageDto> HandleCriptoArbitrage(string criptoName, string bidCurrency, string AskCurrency)
        {
            var usdAsk = await GetBitstampAskPrice(CryptoCodeConstants.BITCOIN, CurrencyConstants.USD);
            var zarBid = await GetValrBidPrice(CryptoCodeConstants.BITCOIN, CurrencyConstants.ZAR);
            var exchangeRate = await GetExchangeRate(CurrencyConstants.USD, CurrencyConstants.ZAR);

            var arbitrageRatio = CalculateArbitrage(zarBid, usdAsk, exchangeRate);

            //save data to database , just realised theres no need to keep this data
            /*await _context.CryptoArbitrages.AddAsync(new CryptoArbitrage
            {
                CryptoName = criptoName,
                BitstampAskPrice = usdAsk,
                ValrBidPrice = zarBid,
                ExchangeRate = exchangeRate,
                ArbitrageRatio = arbitrageRatio,
                Timestamp = DateTime.Now
            });
            //save changes
            await _context.SaveChangesAsync();*/  


            return new ArbitrageDto
            {
                BitstampAskPrice = usdAsk,
                ValrBidPrice = zarBid,
                ExchangeRate = exchangeRate,
                ArbitrageRatio = arbitrageRatio
            };
        }
    }
}