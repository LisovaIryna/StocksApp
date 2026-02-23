using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using Microsoft.Extensions.Options;

namespace StocksApp.ViewComponents;

public class SelectedStockViewComponent :ViewComponent
{
    private readonly TradingOptions _tradingOptions;
    private readonly IBuyOrdersService _stocksService;
    private readonly IFinnhubCompanyProfileService _finnhubService;
    private readonly IConfiguration _configuration;

    public SelectedStockViewComponent (IOptions<TradingOptions> tradingOptions, IBuyOrdersService stocksService, IFinnhubCompanyProfileService finnhubService, IConfiguration configuration)
    {
        _tradingOptions = tradingOptions.Value;
        _stocksService = stocksService;
        _finnhubService = finnhubService;
        _configuration = configuration;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? stockSymbol)
    {
        Dictionary<string, object>? profitDictionary = null;

        if (stockSymbol != null)
        {
            profitDictionary = await _finnhubService.GetCompanyProfile(stockSymbol);
            var stockPriceDictionary = await _finnhubService.GetStockPriceQuote(stockSymbol);
            if (stockPriceDictionary != null && profitDictionary != null)
                profitDictionary.Add("price", stockPriceDictionary["c"]);
        }


        if (profitDictionary != null && profitDictionary.ContainsKey("logo"))
            return View(profitDictionary);
        else
            return Content("");
    }
}
