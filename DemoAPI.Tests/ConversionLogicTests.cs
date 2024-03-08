using DemoAPI.Logic;
using DemoAPI.Model;
using DemoAPI.Repository;
using Microsoft.Extensions.Logging;
using Moq;

namespace DemoAPI.Tests.Logic
{
    [TestClass]
    public class ConversionLogicTests
    {
        private Mock<IConversionRepository> _conversionRepositoryMock;
        private Mock<ILogger<ConversionLogic>> _loggerMock;
        private ConversionLogic _conversionLogic;

        [TestInitialize]
        public void Setup()
        {
            _conversionRepositoryMock = new Mock<IConversionRepository>();
            _loggerMock = new Mock<ILogger<ConversionLogic>>();
            _conversionLogic = new ConversionLogic(_conversionRepositoryMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task ConvertCurrency_ValidRequest_ReturnsValidResponse()
        {
            // Arrange
            var sourceCurrency = "USD";
            var targetCurrency = "EUR";
            var amount = 100m;
            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate { FromCurrency = "USD", ToCurrency = "EUR", Rate = 0.85m }
            };
            _conversionRepositoryMock.Setup(repo => repo.GetExchangeRatesAsync())
                                     .ReturnsAsync(exchangeRates);

            // Act
            var result = await _conversionLogic.ConvertCurrency(sourceCurrency, targetCurrency, amount);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0.85m, result.ExchangeRate);
            Assert.AreEqual(85m, result.ConvertedAmount);
        }

        [TestMethod]
        public async Task ConvertCurrency_InvalidRequest_ReturnsNull()
        {
            // Arrange
            var sourceCurrency = "USD";
            var targetCurrency = "EUR";
            var amount = 100m;
            var exchangeRates = new List<ExchangeRate>
            {
                new ExchangeRate { FromCurrency = "GBP", ToCurrency = "EUR", Rate = 1.2m }
            };
            _conversionRepositoryMock.Setup(repo => repo.GetExchangeRatesAsync())
                                     .ReturnsAsync(exchangeRates);

            // Act
            var result = await _conversionLogic.ConvertCurrency(sourceCurrency, targetCurrency, amount);

            // Assert
            Assert.IsNull(result);
        }
    }
}
