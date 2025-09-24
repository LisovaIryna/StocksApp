using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using StocksApp.Models;
using System.Globalization;

namespace StocksApp.Controllers;

[Route("[controller]")]
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
    [Route("[action]")]
    [Route("~/[controller]")]
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
                StockSymbol = Convert.ToString(profileDictionary["ticker"].ToString()),
                StockName = Convert.ToString(profileDictionary["name"].ToString()),
                Quantity = _tradingOptions.DefaultOrderQuantity ?? 0, 
                Price = Convert.ToDouble(quoteDictionary["c"].ToString(), cultureInfo)
            };
        }

        ViewBag.FinnhubToken = _configuration["FinnhubToken"];

        return View(stockTrade);
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
    {
        buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;

        ModelState.Clear();
        TryValidateModel(buyOrderRequest);
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            StockTrade stockTrade = new()
            {
                StockName = buyOrderRequest.StockName,
                StockSymbol = buyOrderRequest.StockSymbol,
                Quantity = buyOrderRequest.Quantity
            };
            return View("Index", stockTrade);
        }

        BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

        return RedirectToAction(nameof(Orders));
    }

    [Route("[action]")]
    [HttpPost]
    public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
    {
        sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;

        ModelState.Clear();
        TryValidateModel(sellOrderRequest);
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            StockTrade stockTrade = new()
            {
                StockName = sellOrderRequest.StockName,
                StockSymbol = sellOrderRequest.StockSymbol,
                Quantity = sellOrderRequest.Quantity
            };
            return View("Index", stockTrade);
        }

        SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

        return RedirectToAction(nameof(Orders));
    }

    [Route("[action]")]
    public async Task<IActionResult> Orders()
    {
        List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetBuyOrders();
        List<SellOrderResponse> sellOrderResponses = await _stocksService.GetSellOrders();

        Orders orders = new()
        {
            BuyOrders = buyOrderResponses,
            SellOrders = sellOrderResponses
        };

        ViewBag.TradingOptions = _tradingOptions;

        return View(orders);
    }
}
