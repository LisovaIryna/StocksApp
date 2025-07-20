using Microsoft.AspNetCore.Mvc;

namespace StocksApp.Controllers;

public class TradeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
