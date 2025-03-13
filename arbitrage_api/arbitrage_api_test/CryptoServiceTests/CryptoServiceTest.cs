using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using arbitrage_api.Domains;
using arbitrage_api.Services.CryptoServices;
using arbitrage_api.Services.CryptoServices.Dtos;
using arbitrage_api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace arbitrage_api_test.CryptoServiceTests
{
    public class CryptoServiceTest
    {
        // Mock dependencies
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IOptions<ArbitrageSettings>> _arbitrageSettingsMock;
        private readonly DbContextOptions<ArbitrageDbContext> _dbContextOptions;

        public CryptoServiceTest()
        {
            // Mock HttpMessageHandler for HttpClient
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            // Create HttpClient with the mocked handler
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

            // Mock IOptions<ArbitrageSettings>
            _arbitrageSettingsMock = new Mock<IOptions<ArbitrageSettings>>();
            _arbitrageSettingsMock.Setup(x => x.Value).Returns(new ArbitrageSettings
            {
                ExchangeRateApiKey = "test_api_key"
            });

            // Configure in-memory database for ArbitrageDbContext
            _dbContextOptions = new DbContextOptionsBuilder<ArbitrageDbContext>()
                .UseInMemoryDatabase(databaseName: "ArbitrageDb")
                .Options;
        }

        // Test: Verify that GetBitstampAskPrice returns the correct ask price
        [Fact]
        public async Task GetBitstampAskPrice_ShouldReturnAskPrice()
        {
            // Arrange
            var expectedAskPrice = 50000m;
            var responseJson = $"{{\"ask\": \"{expectedAskPrice}\"}}";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseJson)
                });

            using var dbContext = new ArbitrageDbContext(_dbContextOptions);
            var cryptoService = new CryptoService(_httpClient, dbContext, _arbitrageSettingsMock.Object);

            // Act
            var askPrice = await cryptoService.GetBitstampAskPrice("btc", "usd");

            // Assert
            Assert.Equal(expectedAskPrice, askPrice);
        }

        // Test: Verify that GetValrBidPrice returns the correct bid price
        [Fact]
        public async Task GetValrBidPrice_ShouldReturnBidPrice()
        {
            // Arrange
            var expectedBidPrice = 900000m;
            var responseJson = $"{{\"bidPrice\": \"{expectedBidPrice}\"}}";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseJson)
                });

            using var dbContext = new ArbitrageDbContext(_dbContextOptions);
            var cryptoService = new CryptoService(_httpClient, dbContext, _arbitrageSettingsMock.Object);

            // Act
            var bidPrice = await cryptoService.GetValrBidPrice("btc", "zar");

            // Assert
            Assert.Equal(expectedBidPrice, bidPrice);
        }

        // Test: Verify that GetExchangeRate returns the correct exchange rate
        [Fact]
        public async Task GetExchangeRate_ShouldReturnExchangeRate()
        {
            // Arrange
            var expectedExchangeRate = 15m;
            var responseJson = $"{{\"conversion_rate\": {expectedExchangeRate}}}";

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseJson)
                });

            using var dbContext = new ArbitrageDbContext(_dbContextOptions);
            var cryptoService = new CryptoService(_httpClient, dbContext, _arbitrageSettingsMock.Object);

            // Act
            var exchangeRate = await cryptoService.GetExchangeRate("usd", "zar");

            // Assert
            Assert.Equal(expectedExchangeRate, exchangeRate);
        }

        // Test: Verify that CalculateArbitrage returns the correct arbitrage ratio
        [Fact]
        public void CalculateArbitrage_ShouldReturnCorrectRatio()
        {
            // Arrange
            using var dbContext = new ArbitrageDbContext(_dbContextOptions);
            var cryptoService = new CryptoService(_httpClient, dbContext, _arbitrageSettingsMock.Object);

            var zarBid = 900000m;
            var usdAsk = 50000m;
            var exchangeRate = 15m;

            // Act
            var arbitrageRatio = cryptoService.CalculateArbitrage(zarBid, usdAsk, exchangeRate);

            // Assert
            Assert.Equal(1.2m, arbitrageRatio); // 900000 / (50000 * 15) = 1.2
        }

        // Test: Verify that HandleCriptoArbitrage saves data to the database and returns the correct DTO
        [Fact]
        public async Task HandleCriptoArbitrage_ShouldSaveDataAndReturnDto()
        {
            // Arrange
            var expectedAskPrice = 50000m;
            var expectedBidPrice = 900000m;
            var expectedExchangeRate = 15m;

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"ask\": \"{expectedAskPrice}\"}}")
                })
                .Verifiable();

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"bidPrice\": \"{expectedBidPrice}\"}}")
                })
                .Verifiable();

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent($"{{\"conversion_rate\": {expectedExchangeRate}}}")
                })
                .Verifiable();

            using var dbContext = new ArbitrageDbContext(_dbContextOptions);
            var cryptoService = new CryptoService(_httpClient, dbContext, _arbitrageSettingsMock.Object);

            // Act
            var arbitrageDto = await cryptoService.HandleCriptoArbitrage("btc", "zar", "usd");

            // Assert
            Assert.NotNull(arbitrageDto);
            Assert.Equal(expectedAskPrice, arbitrageDto.BitstampAskPrice);
            Assert.Equal(expectedBidPrice, arbitrageDto.ValrBidPrice);
            Assert.Equal(expectedExchangeRate, arbitrageDto.ExchangeRate);
            Assert.Equal(1.2m, arbitrageDto.ArbitrageRatio); // 900000 / (50000 * 15) = 1.2

            // Verify data was saved to the database
            var savedData = await dbContext.CryptoArbitrages.FirstOrDefaultAsync();
            Assert.NotNull(savedData);
            Assert.Equal("btc", savedData.CryptoName);
            Assert.Equal(expectedAskPrice, savedData.BitstampAskPrice);
            Assert.Equal(expectedBidPrice, savedData.ValrBidPrice);
            Assert.Equal(expectedExchangeRate, savedData.ExchangeRate);
            Assert.Equal(1.2m, savedData.ArbitrageRatio);
        }
    }

}

