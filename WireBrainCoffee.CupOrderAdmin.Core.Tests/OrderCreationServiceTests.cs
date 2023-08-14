using Castle.Core.Resource;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WiredBrainCoffee.CupOrderAdmin.Core.DataInterfaces;
using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.Core.Services.OrderCreation;

namespace WireBrainCoffee.CupOrderAdmin.Core.Tests
{
    [TestClass]
    public class OrderCreationServiceTests
    {
        private OrderCreationService _orderCreationService;
        private int _numberOfCupsInStock;

        [TestInitialize]
        public void TestInitialize()
        {
            _numberOfCupsInStock = 10;
            var orderRepositoryMock = new Mock<IOrderRepository>();
            orderRepositoryMock.Setup(x =>
            x.SaveAsync(It.IsAny<Order>())).ReturnsAsync((Order x) => x);

            var coffeeCupRepositoryMock = new Mock<ICoffeeCupRepository>();
            coffeeCupRepositoryMock.Setup(x => x.GetCoffeeCupsInStockCountAsync())
                .ReturnsAsync(_numberOfCupsInStock);

            _orderCreationService = new OrderCreationService(orderRepositoryMock.Object, coffeeCupRepositoryMock.Object);

        }

        [TestMethod]
        public async Task ShouldStoreCreatedOrderInOrderCreationResult()
        {

            var numberOfOrderedCups = 1;
            var customer = new Customer { Id = 99 };

            var orderCreationResult =
                await _orderCreationService.CreateOrderAsync(customer, numberOfOrderedCups);

            Assert.AreEqual(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.IsNotNull(orderCreationResult.CreatedOrder);
            Assert.AreEqual(customer.Id, orderCreationResult.CreatedOrder.CustomerId);

        }

        [TestMethod]
        public async Task ShouldStoreRemainingCupsInStockInOrderCreationResult()
        {
            var numberOfOrderedCups = 3;
            var expectedRemainingCupsInStock = _numberOfCupsInStock - numberOfOrderedCups;
            var customer = new Customer();

            var orderCreationResult =
                await _orderCreationService.CreateOrderAsync(customer, numberOfOrderedCups);

            Assert.AreEqual(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.AreEqual(expectedRemainingCupsInStock, orderCreationResult.RemainingCupsInStock);
        }

        [TestMethod]
        public async Task ShouldReturnStockExceededResultIfNotEnoughCupsInStock()
        {
            var numberOfOrderCups = _numberOfCupsInStock + 1;
            var customer = new Customer();

            var orderCreationResult =
                await _orderCreationService.CreateOrderAsync(customer, numberOfOrderCups);

            Assert.AreEqual(OrderCreationResultCode.StockExceeded, orderCreationResult.ResultCode);
            Assert.AreEqual(_numberOfCupsInStock, orderCreationResult.RemainingCupsInStock);

        }

        [TestMethod]
        public async Task ShouldThrowExceptionIfNumberOfOrderedCupsIsLessThanOne()
        {
            var numberOfOrderCups = 0;
            var customer = new Customer();

            var exception = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>( () =>
                _orderCreationService.CreateOrderAsync(customer, numberOfOrderCups));

            Assert.AreEqual("numberOfOrderedCups", exception.ParamName);

        }

        [TestMethod]
        public async Task ShouldThrowExceptionIfCustomerIsNull() 
        {
            var numberOfOrderCups = 1;

            var exception = await Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() =>
            _orderCreationService.CreateOrderAsync(null, numberOfOrderCups));

            Assert.AreEqual("customer", exception.ParamName);

        }


        [TestMethod]
        public async Task ShouldCalculateCorrectDiscountPercentage() {
            var numberOfOrderedCups = 5;
            var customer = new Customer {Membership = CustomerMembership.Basic};

            var orderCreationResult = await _orderCreationService.CreateOrderAsync(customer, numberOfOrderedCups);

            Assert.AreEqual(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.AreEqual(3, orderCreationResult.CreatedOrder.DiscountInPercent);

        }


        [TestMethod]
        public async Task ShouldCalculateCorrectDiscountPercentage2() {
            var numberOfOrderedCups = 5;
            var customer = new Customer {Membership = CustomerMembership.Basic};

            var orderCreationResult = await _orderCreationService.CreateOrderAsync(customer, numberOfOrderedCups);

            Assert.AreEqual(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.AreEqual(0, orderCreationResult.CreatedOrder.DiscountInPercent);

        }

    }


}