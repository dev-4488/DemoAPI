using DemoAPI.Model;

namespace DemoAPI.Logic
{
    public interface IConversionLogic
    {
        Task<CurrencyConvertResponse> ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount);
    }
}
