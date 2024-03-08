using DemoAPI.Model;
using DemoAPI.Repository;

namespace DemoAPI.Logic
{
    public class ConversionLogic : IConversionLogic
    {
        private readonly IConversionRepository _conversionRateRepository;
        private readonly ILogger<ConversionLogic> _logger;

        public ConversionLogic(IConversionRepository conversionRateRepository, ILogger<ConversionLogic> logger)
        {
            _conversionRateRepository = conversionRateRepository;
            _logger = logger;

        }

        public async Task<CurrencyConvertResponse> ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount)
        {
            _logger.LogInformation($"Converting {amount} {sourceCurrency} to {targetCurrency}...");

            _logger.LogInformation("Loading exchange rates...");
            var exchangeRates = await _conversionRateRepository.GetExchangeRatesAsync();
            var exchangeRate = exchangeRates.FirstOrDefault(x => x.FromCurrency.Equals(sourceCurrency, StringComparison.InvariantCultureIgnoreCase) && x.ToCurrency.Equals(targetCurrency, StringComparison.InvariantCultureIgnoreCase));

            if (exchangeRate != null)
            {
                var convertedAmount = amount * exchangeRate.Rate;
                return new CurrencyConvertResponse
                {
                    ExchangeRate = exchangeRate.Rate,
                    ConvertedAmount = convertedAmount
                };
            }
            else
            {
                return null;
            }
        }
    }
}
