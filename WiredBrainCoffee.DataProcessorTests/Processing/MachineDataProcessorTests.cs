﻿using WiredBrainCoffee.DataProcessor.Data;
using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing
{
    public class MachineDataProcessorTests
    {
        private readonly FakeCoffeeCountStore _coffeeCountStore;
        private readonly MachineDataProcessor _machineDataProcessor;

        public MachineDataProcessorTests()
        {
            _coffeeCountStore = new FakeCoffeeCountStore();
            _machineDataProcessor = new MachineDataProcessor(_coffeeCountStore);

        }

        [Fact]
        public void ShouldSaveCountPerCoffeeType()
        {
            // Arrange
            var items = new[]
            {
                new MachineDataItem("Cappuccino", new DateTime(2023,08,25,8,0,0)),
                new MachineDataItem("Cappuccino", new DateTime(2023,08,25,9,0,0)),
                new MachineDataItem("4", new DateTime(2023,08,25,10,0,0)),
            };

            // Act
            _machineDataProcessor.ProcessItems(items);

            // Assert 
            Assert.Equal(2, _coffeeCountStore.SavedItems.Count);

            var item = _coffeeCountStore.SavedItems[0];
            Assert.Equal("Cappuccino", item.CoffeeType);
            Assert.Equal(2, item.Count); 

            item = _coffeeCountStore.SavedItems[1];
            Assert.Equal("Espresso", item.CoffeeType);
            Assert.Equal(1, item.Count); 
        }


        [Fact]
        public void ShouldClearPreviousCoffeeCount()
        {
            // Arrange
            var items = new[]
            {
                new MachineDataItem("Cappuccino", new DateTime(2023,08,25,8,0,0)),
            };

            // Act
            _machineDataProcessor.ProcessItems(items);
            _machineDataProcessor.ProcessItems(items);

            // Assert 
            Assert.Equal(2, _coffeeCountStore.SavedItems.Count);

            foreach (var item in _coffeeCountStore.SavedItems)
            {
                Assert.Equal("Cappuccino", item.CoffeeType);
                Assert.Equal(1, item.Count);
            }
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
