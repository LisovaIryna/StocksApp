using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.Models;
using System.Globalization;

namespace StocksApp.Controllers;

public class TradeController : Controller
{
    CultureInfo cultureInfo = new("en-US");

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

    [Route("/")]
    public async Task<IActionResult> Index()
    {
        if (string.IsNullOrEmpty(_tradingOptions.DefaultStockSymbol))
            _tradingOptions.DefaultStockSymbol = "MSFT";

        Dictionary<string, object>? profileDictionary = await _finnhubService.GetCompanyProfile(_tradingOptions.DefaultStockSymbol);
        Dictionary<string, object>? quoteDictionary = await _finnhubService.GetStockPriceQuote(_tradingOptions.DefaultStockSymbol);

        StockTrade stockTrade = new()
        {
            StockSymbol = _tradingOptions.DefaultStockSymbol
        };

        if (profileDictionary != null && quoteDictionary != null)
        {
            stockTrade = new StockTrade()
            {
                StockSymbol = Convert.ToString(profileDictionary["ticker"]),
                StockName = Convert.ToString(profileDictionary["name"]),
                Price = Convert.ToDouble(quoteDictionary["c"].ToString(), cultureInfo)
            };
        }

        ViewBag.FinnhubToken = _configuration["FinnhubToken"];

        return View(stockTrade);
    }
}
