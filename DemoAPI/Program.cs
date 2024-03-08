using AspNetCoreRateLimit;
using DemoAPI.Logic;
using DemoAPI.Repository;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IConversionRepository, ConversionRepository>();
builder.Services.AddTransient<IConversionLogic, ConversionLogic>();
builder.Services.AddLogging(builder => builder.AddConsole());
builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.GeneralRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "*",
                Limit = 100, // Number of requests allowed
                Period = "1h" // Time period for the limit (e.g., 1 hour)
            }
        };
});
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.UseRateLimiter();
app.Run();
