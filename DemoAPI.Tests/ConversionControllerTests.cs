using DemoAPI.Controllers;
using DemoAPI.Logic;
using DemoAPI.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace DemoAPI.Tests.Controllers
{
    [TestClass]
    public class ConversionControllerTests
    {
        private Mock<IConversionLogic> _conversionLogicMock;
        private Mock<ILogger<ConversionController>> _loggerMock;
        private ConversionController _controller;

        [TestInitialize]
        public void Setup()
        {
            _conversionLogicMock = new Mock<IConversionLogic>();
            _loggerMock = new Mock<ILogger<ConversionController>>();
            _controller = new ConversionController(_conversionLogicMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public async Task ConvertCurrency_ValidRequest_ReturnsOk()
        {
            // Arrange
            var sourceCurrency = "USD";
            var targetCurrency = "EUR";
            var amount = 100m;
            var expectedResult = new CurrencyConvertResponse
            {
                ExchangeRate = 0.85m,
                ConvertedAmount = 85m
            };
            _conversionLogicMock.Setup(x => x.ConvertCurrency(sourceCurrency, targetCurrency, amount))
                                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ConvertCurrency(sourceCurrency, targetCurrency, amount) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOfType(result.Value, typeof(CurrencyConvertResponse));
            var response = result.Value as CurrencyConvertResponse;
            Assert.AreEqual(expectedResult.ExchangeRate, response.ExchangeRate);
            Assert.AreEqual(expectedResult.ConvertedAmount, response.ConvertedAmount);
        }

        [TestMethod]
        public async Task ConvertCurrency_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var sourceCurrency = "USD";
            var targetCurrency = "EUR";
            var amount = 100m;
            _conversionLogicMock
                .Setup(x => x.ConvertCurrency(sourceCurrency, targetCurrency, amount))
                .ReturnsAsync(It.IsAny<CurrencyConvertResponse>());

            // Act
            var result = await _controller.ConvertCurrency(sourceCurrency, targetCurrency, amount) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Invalid conversion parameters", result.Value);
        }

        [TestMethod]
        public async Task ConvertCurrency_InvalidAmountValue_ReturnsBadRequest() 
        {
            // Arrange
            var sourceCurrency = "USD";
            var targetCurrency = "EUR";
            var amount = -100m;

            // Act
            var result = await _controller.ConvertCurrency(sourceCurrency, targetCurrency, amount) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
            Assert.AreEqual("Invalid conversion parameters", result.Value);
          
        }
    }
}
