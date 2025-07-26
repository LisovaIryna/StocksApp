using ServiceContracts.DTO;
using Services;

namespace Tests;

public class StocksServiceTest
{
    private readonly IStocksService _stocksService;

    public StocksServiceTest()
    {
        _stocksService = new StocksService();
    }

    #region CreateBuyOrder

    [Fact]
    public void CreateBuyOrder_NullBuyOrder()
    {
        // Arrange
        BuyOrderRequest? buyOrderRequest = null;

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            // Act
            await _stocksService.CreateBuyOrder(buyOrderRequest);
        });
    }

    [Theory]
    [InlineData(0)]
    public void CreateBuyOrder_BuyOrderQuantityIsLessThanMinimum(uint buyOrderQuantity)
    {
        // Arrange
        BuyOrderRequest? buyOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            Quantity = buyOrderQuantity,
            Price = 100
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateBuyOrder(buyOrderRequest);
        });
    }

    [Theory]
    [InlineData(100001)]
    public void CreateBuyOrder_BuyOrderQuantityIsMoreThanMaximum(uint buyOrderQuantity)
    {
        // Arrange
        BuyOrderRequest? buyOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            Quantity = buyOrderQuantity,
            Price = 100
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateBuyOrder(buyOrderRequest);
        });
    }

    [Theory]
    [InlineData(0)]
    public void CreateBuyOrder_BuyOrderPriceIsLessThanMinimum(double buyOrderPrice)
    {
        // Arrange
        BuyOrderRequest? buyOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            Quantity = 100,
            Price = buyOrderPrice
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateBuyOrder(buyOrderRequest);
        });
    }

    [Theory]
    [InlineData(10001)]
    public void CreateBuyOrder_BuyOrderPriceIsMoreThanMaximum(double buyOrderPrice)
    {
        // Arrange
        BuyOrderRequest? buyOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            Quantity = 100,
            Price = buyOrderPrice
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateBuyOrder(buyOrderRequest);
        });
    }

    [Fact]
    public void CreateBuyOrder_BuyOrderStockSymbolIsNull()
    {
        // Arrange
        BuyOrderRequest? buyOrderRequest = new()
        {
            StockSymbol = null,
            StockName = "Microsoft",
            Quantity = 100,
            Price = 100
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateBuyOrder(buyOrderRequest);
        });
    }

    [Fact]
    public void CreateBuyOrder_BuyOrderDateAndTimeOfOrderOlderThan2000()
    {
        // Arrange
        BuyOrderRequest? buyOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"),
            Quantity = 100,
            Price = 100
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateBuyOrder(buyOrderRequest);
        });
    }

    [Fact]
    public async void CreateBuyOrder_BuyOrderValidData()
    {
        // Arrange
        BuyOrderRequest? buyOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = Convert.ToDateTime("2025-07-26"),
            Quantity = 100,
            Price = 100
        };

        // Act
        BuyOrderResponse buyOrderResponse_FromCreate = await _stocksService.CreateBuyOrder(buyOrderRequest);

        // Arrange
        Assert.NotEqual(Guid.Empty, buyOrderResponse_FromCreate.BuyOrderID);
    }

    #endregion

    #region CreateSellOrder

    [Fact]
    public void CreateSellOrder_NullSellOrder()
    {
        // Arrange
        SellOrderRequest? sellOrderRequest = null;

        // Assert
        Assert.ThrowsAsync<ArgumentNullException>(async () =>
        {
            // Act
            await _stocksService.CreateSellOrder(sellOrderRequest);
        });
    }

    [Theory]
    [InlineData(0)]
    public void CreateSellOrder_SellOrderQuantityIsLessThanMinimum(uint sellOrderQuantity)
    {
        // Arrange
        SellOrderRequest? sellOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            Quantity = sellOrderQuantity,
            Price = 100
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateSellOrder(sellOrderRequest);
        });
    }

    [Theory]
    [InlineData(100001)]
    public void CreateSellOrder_SellOrderQuantityIsMoreThanMaximum(uint sellOrderQuantity)
    {
        // Arrange
        SellOrderRequest? sellOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            Quantity = sellOrderQuantity,
            Price = 100
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateSellOrder(sellOrderRequest);
        });
    }

    [Theory]
    [InlineData(0)]
    public void CreateSellOrder_SellOrderPriceIsLessThanMinimum(double sellOrderPrice)
    {
        // Arrange
        SellOrderRequest? sellOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            Quantity = 100,
            Price = sellOrderPrice
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateSellOrder(sellOrderRequest);
        });
    }

    [Theory]
    [InlineData(10001)]
    public void CreateSellOrder_SellOrderPriceIsMoreThanMaximum(double sellOrderPrice)
    {
        // Arrange
        SellOrderRequest? sellOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            Quantity = 100,
            Price = sellOrderPrice
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateSellOrder(sellOrderRequest);
        });
    }

    [Fact]
    public void CreateSellOrder_SellOrderStockSymbolIsNull()
    {
        // Arrange
        SellOrderRequest? sellOrderRequest = new()
        {
            StockSymbol = null,
            StockName = "Microsoft",
            Quantity = 100,
            Price = 100
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateSellOrder(sellOrderRequest);
        });
    }

    [Fact]
    public void CreateSellOrder_SellOrderDateAndTimeOfOrderOlderThan2000()
    {
        // Arrange
        SellOrderRequest? sellOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"),
            Quantity = 100,
            Price = 100
        };

        // Assert
        Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            // Act
            await _stocksService.CreateSellOrder(sellOrderRequest);
        });
    }

    [Fact]
    public async void CreateSellOrder_SellOrderValidData()
    {
        // Arrange
        SellOrderRequest? sellOrderRequest = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = Convert.ToDateTime("2025-07-26"),
            Quantity = 100,
            Price = 100
        };

        // Act
        SellOrderResponse sellOrderResponse_FromCreate = await _stocksService.CreateSellOrder(sellOrderRequest);

        // Arrange
        Assert.NotEqual(Guid.Empty, sellOrderResponse_FromCreate.SellOrderID);
    }

    #endregion

    #region GetAllBuyOrders

    [Fact]
    public async void GetAllBuyOrders_DefaultListIsEmpty()
    {
        // Act
        List<BuyOrderResponse> buyOrders_FromGet = await _stocksService.GetBuyOrders();

        // Assert
        Assert.Empty(buyOrders_FromGet);
    }

    [Fact]
    public async void GetAllBuyOrders_WithFewBuyOrders()
    {
        // Arrange
        BuyOrderRequest buyOrderRequest_1 = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = DateTime.Parse("2025-07-26 9:00"),
            Quantity = 100,
            Price = 100
        };
        BuyOrderRequest buyOrderRequest_2 = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = DateTime.Parse("2025-07-26 9:00"),
            Quantity = 100,
            Price = 100
        };

        List<BuyOrderRequest> buyOrderRequests = new()
        {
            buyOrderRequest_1,
            buyOrderRequest_2
        };

        List<BuyOrderResponse> buyOrderResponses_FromAdd = new();
        foreach (BuyOrderRequest buyOrderRequest in buyOrderRequests)
        {
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);
            buyOrderResponses_FromAdd.Add(buyOrderResponse);
        }

        // Act
        List<BuyOrderResponse> buyOrderResponse_FromGet = await _stocksService.GetBuyOrders();

        // Assert
        foreach (BuyOrderResponse buyOrderResponse_FromAdd in buyOrderResponses_FromAdd)
        {
            Assert.Contains(buyOrderResponse_FromAdd, buyOrderResponse_FromGet);
        }
    }

    #endregion

    #region GetAllSellOrders

    [Fact]
    public async void GetAllSellOrders_DefaultListIsEmpty()
    {
        // Act
        List<SellOrderResponse> sellOrders_FromGet = await _stocksService.GetSellOrders();

        // Assert
        Assert.Empty(sellOrders_FromGet);
    }

    [Fact]
    public async void GetAllSellOrders_WithFewSellOrders()
    {
        // Arrange
        SellOrderRequest sellOrderRequest_1 = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = DateTime.Parse("2025-07-26 9:00"),
            Quantity = 100,
            Price = 100
        };
        SellOrderRequest sellOrderRequest_2 = new()
        {
            StockSymbol = "MSFT",
            StockName = "Microsoft",
            DateAndTimeOfOrder = DateTime.Parse("2025-07-26 9:00"),
            Quantity = 100,
            Price = 100
        };

        List<SellOrderRequest> sellOrderRequests = new()
        {
            sellOrderRequest_1,
            sellOrderRequest_2
        };

        List<SellOrderResponse> sellOrderResponses_FromAdd = new();
        foreach (SellOrderRequest sellOrderRequest in sellOrderRequests)
        {
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);
            sellOrderResponses_FromAdd.Add(sellOrderResponse);
        }

        // Act
        List<SellOrderResponse> sellOrderResponse_FromGet = await _stocksService.GetSellOrders();

        // Assert
        foreach (SellOrderResponse sellOrderResponse_FromAdd in sellOrderResponses_FromAdd)
        {
            Assert.Contains(sellOrderResponse_FromAdd, sellOrderResponse_FromGet);
        }
    }

    #endregion
}