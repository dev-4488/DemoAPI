using DemoAPI.Model;
using Newtonsoft.Json;

namespace DemoAPI.Repository
{
    public class ConversionRepository : IConversionRepository
    {
        private readonly List<ExchangeRate> exchangeRates;
        private readonly ILogger _logger;

        public ConversionRepository(ILogger<ConversionRepository> logger)
        {
            _logger = logger;
            exchangeRates = GetExchangeRatesFromJson();
            
        }

        public async Task<List<ExchangeRate>> GetExchangeRatesAsync()
        {
            return await Task.FromResult(exchangeRates);
        }

        private List<ExchangeRate> GetExchangeRatesFromJson()
        {
            _logger.LogInformation("Loading exchange rates...");

            // Read the JSON file
            string json = File.ReadAllText("exchangeRates.json");

            // Deserialize JSON to dictionary
            Dictionary<string, decimal> rates = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(json);

            // Convert dictionary to list of ExchangeRate objects
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();
            foreach (var kvp in rates)
            {
                string[] currencies = kvp.Key.Split("_TO_");
                exchangeRates.Add(new ExchangeRate
                {
                    FromCurrency = currencies[0],
                    ToCurrency = currencies[1],
                    Rate = kvp.Value
                });
            }

            _logger.LogInformation("Exchange rates loaded successfully.");

            return exchangeRates;
        }
    }
}
