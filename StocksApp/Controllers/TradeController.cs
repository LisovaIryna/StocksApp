using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
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

    [Route("[action]/{stockSymbol}")]
    [Route("~/[controller]/{stockSymbol}")]
    public async Task<IActionResult> Index(string stockSymbol)
    {
        if (string.IsNullOrEmpty(stockSymbol))
            stockSymbol = "MSFT";

        Dictionary<string, object>? profileDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);
        Dictionary<string, object>? quoteDictionary = await _finnhubService.GetStockPriceQuote(stockSymbol);

        StockTrade stockTrade = new()
        {
            StockSymbol = stockSymbol
        };

        if (profileDictionary != null && quoteDictionary != null)
        {
            stockTrade = new StockTrade()
            {
                StockSymbol = profileDictionary["ticker"].ToString(),
                StockName = profileDictionary["name"].ToString(),
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

    [Route("OrdersPDF")]
    public async Task<IActionResult> OrdersPDF()
    {
        List<IOrderResponse> orders = new();

        orders.AddRange(await _stocksService.GetBuyOrders());
        orders.AddRange(await _stocksService.GetSellOrders());

        orders = orders.OrderByDescending(temp => temp.DateAndTimeOfOrder).ToList();

        ViewBag.TradingOptions = _tradingOptions;

        return new ViewAsPdf("OrdersPDF", orders, ViewData)
        {
            PageMargins = new Rotativa.AspNetCore.Options.Margins()
            {
                Top = 20,
                Right = 20,
                Bottom = 20,
                Left = 20
            },
            PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
        };
    }
}
