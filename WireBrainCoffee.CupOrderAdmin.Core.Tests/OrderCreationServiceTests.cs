using Moq;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.Core.Model.Enums;
using WiredBrainCoffee.CupOrderAdmin.Core.Services.OrderCreation;
using Xunit;

namespace WireBrainCoffee.CupOrderAdmin.Core.Tests
{
    public class OrderCreationServiceTests
    {
        private OrderCreationService _orderCreationService;
        private int _numberOfCupsInStock;

        public OrderCreationServiceTests()
        {
            _numberOfCupsInStock = 10;
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(x =>
            x.SaveAsync(It.IsAny<Order>())).ReturnsAsync((Order x) => x);

            var coffeeCupRepositoryMock = new Mock<ICoffeeCupRepository>();
            coffeeCupRepositoryMock.Setup(x => x.GetCoffeeCupsInStockCountAsync())
                .ReturnsAsync(_numberOfCupsInStock);
            coffeeCupRepositoryMock.Setup(x => x.GetCoffeeCupsInStockAsync(It.IsAny<int>()))
                .ReturnsAsync((int numberOfOrderedCups) => Enumerable.Range(1,numberOfOrderedCups)
                                                                                 .Select(x => new CoffeeCup()));

            _orderCreationService = new OrderCreationService(orderRepositoryMock.Object, coffeeCupRepositoryMock.Object);

        }

        [Fact]
        public async Task ShouldStoreCreatedOrderInOrderCreationResult()
        {

            var numberOfOrderedCups = 1;
            var customer = new Customer { Id = 99 };

            var orderCreationResult =
                await _orderCreationService.CreateOrderAsync(customer, numberOfOrderedCups);

            Assert.Equal(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.NotNull(orderCreationResult.CreatedOrder);
            Assert.Equal(customer.Id, orderCreationResult.CreatedOrder.CustomerId);

        }

        [Fact]
        public async Task ShouldStoreRemainingCupsInStockInOrderCreationResult()
        {
            var numberOfOrderedCups = 3;
            var expectedRemainingCupsInStock = _numberOfCupsInStock - numberOfOrderedCups;
            var customer = new Customer();

            var orderCreationResult =
                await _orderCreationService.CreateOrderAsync(customer, numberOfOrderedCups);

            Assert.Equal(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.Equal(expectedRemainingCupsInStock, orderCreationResult.RemainingCupsInStock);
        }

        [Fact]
        public async Task ShouldReturnStockExceededResultIfNotEnoughCupsInStock()
        {
            var numberOfOrderCups = _numberOfCupsInStock + 1;
            var customer = new Customer();

            var orderCreationResult =
                await _orderCreationService.CreateOrderAsync(customer, numberOfOrderCups);

            Assert.Equal(OrderCreationResultCode.StockExceeded, orderCreationResult.ResultCode);
            Assert.Equal(_numberOfCupsInStock, orderCreationResult.RemainingCupsInStock);

        }

        [Fact]
        public async Task ShouldThrowExceptionIfNumberOfOrderedCupsIsLessThanOne()
        {
            var numberOfOrderCups = 0;
            var customer = new Customer();

            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
                _orderCreationService.CreateOrderAsync(customer, numberOfOrderCups));

            Assert.Equal("numberOfOrderedCups", exception.ParamName);

        }

        [Fact]
        public async Task ShouldThrowExceptionIfCustomerIsNull()
        {
            var numberOfOrderCups = 1;

            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() =>
            _orderCreationService.CreateOrderAsync(null, numberOfOrderCups));

            Assert.Equal("customer", exception.ParamName);

        }


        [Theory]
        [InlineData(3, 5, CustomerMembership.Basic)]
        [InlineData(0, 4, CustomerMembership.Basic)]
        [InlineData(0, 1, CustomerMembership.Basic)]
        [InlineData(8, 5, CustomerMembership.Premium)]
        [InlineData(5, 4, CustomerMembership.Premium)]
        [InlineData(5, 1, CustomerMembership.Premium)]
        public void ShouldCalculateCorrectDiscountPercentage(double expectedDiscountInPercent, int numberOfOrderedCups, CustomerMembership customerMembership)
        {
            var discountPercentage = OrderCreationService.CalculateDiscountPercentage(customerMembership, numberOfOrderedCups);

            Assert.Equal(expectedDiscountInPercent, discountPercentage);

        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionIfOrderRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new OrderCreationService(null, new Mock<ICoffeeCupRepository>().Object));

            Assert.Equal("orderRepository", exception.ParamName);
        }

        [Fact]
        public void ShouldThrowArgumentNullExceptionIfCoffeeCupRepositoryIsNull()
        {
            var exception = Assert.Throws<ArgumentNullException>(
                () => new OrderCreationService(new Mock<IOrderRepository>().Object, null));

            Assert.Equal("coffeeCupRepository", exception.ParamName);
        }



    }


}