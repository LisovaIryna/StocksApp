using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;

namespace StocksApp.Controllers;

public class TradeController : Controller
{
    private readonly TradingOptions _tradingOptions;
    private readonly IStocksService _stocksService;
    private readonly IFinnhubService _finnhubService;
    private readonly IConfiguration _configuration;

    public TradeController(IOptions<TradingOptions> tradingOptions, IStocksService stocksService, IFinnhubService finnhubService, IConfiguration configuration)
    {
        _tradingOptions = tradingOptions.Value;
        _stocksService = stocksService;
        _finnhubService = finnhubService;
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }
}
