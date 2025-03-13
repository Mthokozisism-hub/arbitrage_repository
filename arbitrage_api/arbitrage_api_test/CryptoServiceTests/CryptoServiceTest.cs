using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using arbitrage_api.Services.CryptoServices;
using arbitrage_api.Services.CryptoServices.Dtos;
using arbitrage_api.Utils;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Xunit;

namespace arbitrage_api_test.CryptoServiceTests
{
    public class CryptoServiceTest
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly HttpClient _httpClient;
        private readonly Mock<IOptions<ArbitrageSettings>> _arbitrageSettingsMock;
        private readonly CryptoService _cryptoService;

        public CryptoServiceTest()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _arbitrageSettingsMock = new Mock<IOptions<ArbitrageSettings>>();

            // Set up default mock values for ArbitrageSettings
            _arbitrageSettingsMock.Setup(x => x.Value).Returns(new ArbitrageSettings
            {
                ExchangeRateApiKey = "14ca7119e4a4fd319568a10e"
            });

            _cryptoService = new CryptoService(_httpClient, _arbitrageSettingsMock.Object);

        }

        [Fact]
        public async Task GetBitstampAskPrice_ReturnsAskPrice()
        {
            // Arrange
            var expectedAskPrice = 50000.00m;
            var responseContent = JsonSerializer.Serialize(new BitstampResponseDto { Ask = expectedAskPrice });

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _cryptoService.GetBitstampAskPrice("BTC", "USD");

            // Assert
            Assert.Equal(expectedAskPrice, result);
        }

        [Fact]
        public async Task GetValrBidPrice_ReturnsBidPrice()
        {
            // Arrange
            var expectedBidPrice = 700000.00m;
            var responseContent = JsonSerializer.Serialize(new ValrResponseDto { Bid = expectedBidPrice });

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _cryptoService.GetValrBidPrice("BTC", "ZAR");

            // Assert
            Assert.Equal(expectedBidPrice, result);
        }

        [Fact]
        public async Task GetExchangeRate_ReturnsConversionRate()
        {
            // Arrange
            var expectedConversionRate = 18.3491m;
            var responseContent = JsonSerializer.Serialize(new ExchangeRateResponseDto { ConversionRate = expectedConversionRate });

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent)
                });

            // Act
            var result = await _cryptoService.GetExchangeRate("USD", "ZAR");

            // Assert
            Assert.Equal(expectedConversionRate, result);
        }

        [Fact]
        public async Task GetBitstampAskPrice_ThrowsException_WhenApiFails()
        {
            // Arrange
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => _cryptoService.GetBitstampAskPrice("BTC", "USD"));
        }
    }

}

