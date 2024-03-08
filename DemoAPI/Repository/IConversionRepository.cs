using DemoAPI.Model;

namespace DemoAPI.Repository
{
    public interface IConversionRepository
    {
        Task<List<ExchangeRate>> GetExchangeRatesAsync();
    }
}
