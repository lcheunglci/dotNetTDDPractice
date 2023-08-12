using WiredBrainCoffee.CupOrderAdmin.Core.Model;
using WiredBrainCoffee.CupOrderAdmin.Core.Services.OrderCreation;

namespace WireBrainCoffee.CupOrderAdmin.Core.Tests
{
    [TestClass]
    public class OrderCreationServiceTests
    {
        [TestMethod]
        public async Task ShouldStoreCreatedOrderInOrderCreationResult()
        {
            var orderCreationService = new OrderCreationService(null, null);

            var numberOfOrderedCups = 1;
            var customer = new Customer();


            var orderCreationResult =
                await orderCreationService.CreateOrderAsync(customer, numberOfOrderedCups);

            Assert.AreEqual(OrderCreationResultCode.Success, orderCreationResult.ResultCode);
            Assert.IsNotNull(orderCreationResult.CreatedOrder);
            Assert.AreEqual(customer.Id, orderCreationResult.CreatedOrder.CustomerId);

        }
    }
}