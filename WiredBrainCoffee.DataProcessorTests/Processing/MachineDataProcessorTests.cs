using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing
{
    public class MachineDataProcessorTests
    {
        [Fact]
        public void ShouldSaveCountPerCoffeeType()
        {
            // Arrange
            var coffeeCountStore = new FakeCoffeeCountStore();
            var machineDataProcessor = new MachineDataProcessor(coffeeCountStore);
            var items = new[]
            {
                new MachineDataItem("Cappuccino", new DateTime(2023,08,25,8,0,0)),
                new MachineDataItem("Cappuccino", new DateTime(2023,08,25,9,0,0)),
                new MachineDataItem("4", new DateTime(2023,08,25,10,0,0)),
            };

            // Act
            machineDataProcessor.ProcessItems(items);

            // Assert 
            Assert.Equal(2, coffeeCountStore.SavedItems.Count);

            var item = coffeeCountStore.SavedItems[0];
            Assert.Equal("Cappuccino", item.CoffeeType);
            Assert.Equal(2, item.Count); 

            item = coffeeCountStore.SavedItems[1];
            Assert.Equal("Espresso", item.CoffeeType);
            Assert.Equal(1, item.Count); 
        }
    }

    public class FakeCoffeeCountStore : ICoffeeCountStore
    {
        public List<CoffeeCountItem> SavedItems { get; } = new();

        public void Save(CoffeeCountItem item)
        {
            SavedItems.Add(item);
        }
    }
}
