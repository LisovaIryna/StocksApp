using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace StocksApp.Controllers;

public class TradeController : Controller
{
    private readonly TradingOptions _tradingOptions;

    public TradeController(IOptions<TradingOptions> tradingOptions)
    {
        _tradingOptions = tradingOptions.Value;
    }

    public IActionResult Index()
    {
        return View();
    }
}
