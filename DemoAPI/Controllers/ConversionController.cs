using DemoAPI.Logic;
using DemoAPI.Model;
using Microsoft.AspNetCore.Mvc;

namespace DemoAPI.Controllers
{
    [ApiController]
    public class ConversionController : ControllerBase
    {
        private readonly IConversionLogic _currencyConversionLogic;
        private readonly ILogger<ConversionController> _logger;
        public ConversionController(IConversionLogic currencyConversionLogic, ILogger<ConversionController> logger)
        {
            _currencyConversionLogic = currencyConversionLogic;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceCurrency"></param>
        /// <param name="targetCurrency"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency([FromQuery] string sourceCurrency, [FromQuery] string targetCurrency, [FromQuery][ModelBinder(BinderType = typeof(DecimalModelBinder))] decimal amount)
        {
            _logger.LogInformation($"Converting {amount} {sourceCurrency} to {targetCurrency}");

            try
            {
                var response = await _currencyConversionLogic.ConvertCurrency(sourceCurrency, targetCurrency, amount);
                if (response == null)
                {
                    _logger.LogError("Invalid conversion parameters");
                    return BadRequest("Invalid conversion parameters");
                }

                _logger.LogInformation($"Conversion successful: {amount} {sourceCurrency} = {response.ConvertedAmount} {targetCurrency}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
