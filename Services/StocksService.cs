using ServiceContracts.DTO;
using Services.Helpers;

namespace Services;

public class StocksService : IStocksService
{
    private readonly List<BuyOrder> _buyOrders;
    private readonly List<SellOrder> _sellOrders;

    public StocksService()
    {
        _buyOrders = new();
        _sellOrders = new();
    }

    public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
    {
        if (buyOrderRequest == null)
            throw new ArgumentNullException(nameof(buyOrderRequest));

        ValidationHelper.ModelValidation(buyOrderRequest);

        BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();

        buyOrder.BuyOrderID = Guid.NewGuid();

        _buyOrders.Add(buyOrder);

        return Task.FromResult(buyOrder.ToBuyOrderResponse());
    }

    public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
    {
        if (sellOrderRequest == null)
            throw new ArgumentNullException(nameof(sellOrderRequest));

        ValidationHelper.ModelValidation(sellOrderRequest);

        SellOrder sellOrder = sellOrderRequest.ToSellOrder();

        sellOrder.SellOrderID = Guid.NewGuid();

        _sellOrders.Add(sellOrder);

        return Task.FromResult(sellOrder.ToSellOrderResponse());
    }

    public Task<List<BuyOrderResponse>> GetBuyOrders()
    {
        return Task.FromResult(_buyOrders
            .OrderByDescending(temp => temp.DateAndTimeOfOrder)
            .Select(temp => temp.ToBuyOrderResponse()).ToList());
    }

    public Task<List<SellOrderResponse>> GetSellOrders()
    {
        return Task.FromResult(_sellOrders
            .OrderByDescending(temp => temp.DateAndTimeOfOrder)
            .Select(temp => temp.ToSellOrderResponse()).ToList());
    }
}
