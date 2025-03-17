using arbitrage_api.Services.CryptoServices;
using arbitrage_api.Services.CryptoServices.Dtos;
using arbitrage_api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace arbitrage_api.Controllers
{
    // Marks this class as an API controller
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    // Defines the base route for all endpoints in this controller
    [Route("Api/[controller]")]
    public class ArbitrageController : ControllerBase
    {
        // Service responsible for fetching and calculating crypto arbitrage data
        private readonly ICryptoService _cryptoPriceService;

        // Constructor for dependency injection
        public ArbitrageController(ICryptoService cryptoPriceService)
        {
            // Inject and store the crypto price service
            _cryptoPriceService = cryptoPriceService;
        }

        // Endpoint to get Bitcoin arbitrage data
        [HttpGet("Bitcoin")]
        public async Task<IActionResult> GetBitcoinArbitrage()
        {
            // Fetch and calculate Bitcoin arbitrage data using the crypto price service
            var arbitrageDto = await _cryptoPriceService.HandleCriptoArbitrage(
                CryptoCodeConstants.BITCOIN, // Cryptocurrency code for Bitcoin
                CurrencyConstants.ZAR,       // Target currency (South African Rand)
                CurrencyConstants.USD        // Base currency (US Dollar)
            );

            // Return the arbitrage data as a response
            return Ok(arbitrageDto);
        }

        // Endpoint to get XRP arbitrage data

        [HttpGet("Xrp")]
        public async Task<IActionResult> GetXrpArbitrage()
        {
            // Fetch and calculate XRP arbitrage data using the crypto price service
            var arbitrageDto = await _cryptoPriceService.HandleCriptoArbitrage(
                CryptoCodeConstants.XRP, // Cryptocurrency code for XRP
                CurrencyConstants.ZAR,   // Target currency (South African Rand)
                CurrencyConstants.USD   // Base currency (US Dollar)
            );

            // Return the arbitrage data as a response
            return Ok(arbitrageDto);
        }
    }
}