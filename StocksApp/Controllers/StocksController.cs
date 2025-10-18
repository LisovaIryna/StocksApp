using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace StocksApp.Controllers;

[Route("[controller]")]
public class StocksController : Controller
{
    private readonly TradingOptions _tradingOptions;
    private readonly IFinnhubService _finnhubService;

    public StocksController(IOptions<TradingOptions> tradingOptions, IFinnhubService finnhubService)
    {
        _tradingOptions = tradingOptions.Value;
        _finnhubService = finnhubService;
    }
}
