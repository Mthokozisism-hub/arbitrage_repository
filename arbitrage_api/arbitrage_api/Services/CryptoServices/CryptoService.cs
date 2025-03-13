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

        // Configuration settings for arbitrage, such as API keys and other parameters.
        private readonly ArbitrageSettings _arbitrageSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="CryptoService"/> class.
        /// </summary>
        /// <param name="httpClient">The HttpClient instance for making API requests.</param>
        /// <param name="arbitrageSettings">The configuration settings for arbitrage.</param>
        public CryptoService(HttpClient httpClient, IOptions<ArbitrageSettings> arbitrageSettings)
        {
            _httpClient = httpClient;
            _arbitrageSettings = arbitrageSettings.Value;
        }

        /// <summary>
        /// Fetches the ask price for a specified cryptocurrency and currency pair from Bitstamp.
        /// </summary>
        /// <param name="cripto">The cryptocurrency symbol (e.g., "btc").</param>
        /// <param name="currency">The currency symbol (e.g., "usd").</param>
        /// <returns>The ask price as a decimal value.</returns>
        public async Task<decimal> GetBitstampAskPrice(string cripto, string currency)
        {
            var url = $"https://www.bitstamp.net/api/v2/ticker/{cripto}{currency}/";
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
            return response.Bid;
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
            return response.ConversionRate;
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
    }
}