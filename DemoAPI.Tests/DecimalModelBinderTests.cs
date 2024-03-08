using System.Globalization;
using DemoAPI.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;

namespace DemoAPI.Tests.ModelBinders
{
    [TestClass]
    public class DecimalModelBinderTests
    {
        [TestMethod]
        public async Task BindModelAsync_ValidDecimal_ReturnsSuccess()
        {
            // Arrange
            var valueProviderMock = new Mock<IValueProvider>();
            valueProviderMock.Setup(vp => vp.GetValue("amount"))
                             .Returns(new ValueProviderResult("123.45", CultureInfo.InvariantCulture));

            var bindingContext = new DefaultModelBindingContext
            {
                ModelName = "amount",
                ValueProvider = valueProviderMock.Object,
                ModelState = new ModelStateDictionary(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(decimal))
            };

            var binder = new DecimalModelBinder();

            // Act
            await binder.BindModelAsync(bindingContext);

            // Assert
            Assert.IsTrue(bindingContext.Result.IsModelSet);
            Assert.AreEqual((decimal)123.45, bindingContext.Result.Model);
        }

        [TestMethod]
        public async Task BindModelAsync_InvalidDecimal_ReturnsError()
        {
            // Arrange
            var valueProviderMock = new Mock<IValueProvider>();
            valueProviderMock.Setup(vp => vp.GetValue("amount"))
                             .Returns(new ValueProviderResult("abc", CultureInfo.InvariantCulture));

            var bindingContext = new DefaultModelBindingContext
            {
                ModelName = "amount",
                ValueProvider = valueProviderMock.Object,
                ModelState = new ModelStateDictionary(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(decimal))
            };

            var binder = new DecimalModelBinder();

            // Act
            await binder.BindModelAsync(bindingContext);

            // Assert
            Assert.IsFalse(bindingContext.Result.IsModelSet);
            Assert.IsNotNull(bindingContext.ModelState["amount"]);
            Assert.AreEqual("Invalid amount. Amount must be a valid decimal.", bindingContext.ModelState["amount"].Errors[0].ErrorMessage);
        }

        [TestMethod]
        public async Task BindModelAsync_NegativeDecimal_ReturnsError()
        {
            // Arrange
            var valueProviderMock = new Mock<IValueProvider>();
            valueProviderMock.Setup(vp => vp.GetValue("amount"))
                             .Returns(new ValueProviderResult("-100", CultureInfo.InvariantCulture));

            var bindingContext = new DefaultModelBindingContext
            {
                ModelName = "amount",
                ValueProvider = valueProviderMock.Object,
                ModelState = new ModelStateDictionary(),
                ModelMetadata = new EmptyModelMetadataProvider().GetMetadataForType(typeof(decimal))
            };

            var binder = new DecimalModelBinder();

            // Act
            await binder.BindModelAsync(bindingContext);

            // Assert
            Assert.IsFalse(bindingContext.Result.IsModelSet);
            Assert.IsNotNull(bindingContext.ModelState["amount"]);
            Assert.AreEqual("Invalid amount. Amount must be greater than 0.", bindingContext.ModelState["amount"].Errors[0].ErrorMessage);
        }
    }
}
