using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace DemoAPI.Model
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Amount is required");
                return Task.CompletedTask;
            }

            var valueAsString = valueProviderResult.FirstValue;

            if (!decimal.TryParse(valueAsString, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal result))
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid amount. Amount must be a valid decimal.");
                return Task.CompletedTask;
            }

            if(result <= 0)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Invalid amount. Amount must be greater than 0.");
                return Task.CompletedTask;
            }

            bindingContext.Result = ModelBindingResult.Success(result);
            return Task.CompletedTask;
        }
    }
}
