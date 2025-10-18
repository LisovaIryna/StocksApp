using RepositoryContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services;

public class StocksService : IStocksService
{
    private readonly IStocksRepository _stocksRepository;

    public StocksService(IStocksRepository stocksRepository)
    {
        _stocksRepository = stocksRepository;
    }

    public async Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
    {
        if (buyOrderRequest == null)
            throw new ArgumentNullException(nameof(buyOrderRequest));

        ValidationHelper.ModelValidation(buyOrderRequest);

        BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

        buyOrder.BuyOrderID = Guid.NewGuid();

        BuyOrder buyOrderRepository = await _stocksRepository.CreateBuyOrder(buyOrder);

        return buyOrder.ToBuyOrderResponse();
    }

    public async Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
    {
        if (sellOrderRequest == null)
            throw new ArgumentNullException(nameof(sellOrderRequest));

        ValidationHelper.ModelValidation(sellOrderRequest);

        SellOrder sellOrder = sellOrderRequest.ToSellOrder();

        sellOrder.SellOrderID = Guid.NewGuid();

        SellOrder sellOrderRepository = await _stocksRepository.CreateSellOrder(sellOrder);

        return sellOrder.ToSellOrderResponse();
    }

    public async Task<List<BuyOrderResponse>> GetBuyOrders()
    {
        return (await _stocksRepository.GetBuyOrders())
            .Select(temp => temp.ToBuyOrderResponse()).ToList();
    }

    public async Task<List<SellOrderResponse>> GetSellOrders()
    {
        return (await _stocksRepository.GetSellOrders())
            .Select(temp => temp.ToSellOrderResponse()).ToList();
    }
}
