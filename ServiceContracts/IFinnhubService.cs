namespace ServiceContracts;

/// <summary>
/// Represents service that makes HTTP requests to finnhub.io
/// </summary>
public interface IFinnhubService
{
    /// <summary>
    /// Returns company details
    /// </summary>
    /// <param name="stockSymbol">Stock symbol to search</param>
    /// <returns>Dictionary that contains company details</returns>
    Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol);

    /// <summary>
    /// Returns stock price details
    /// </summary>
    /// <param name="stockSymbol">Stock symbol to search</param>
    /// <returns>Dictionary that contains stock price details</returns>
    Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol);
}
