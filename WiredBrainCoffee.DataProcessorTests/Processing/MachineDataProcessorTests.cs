using WiredBrainCoffee.DataProcessor.Model;

namespace WiredBrainCoffee.DataProcessor.Processing
{
    public class MachineDataProcessorTests
    {
        [Fact]
        public void ShouldSaveCountPerCoffeeType()
        {
            // Arrange
            var machineDataProcessor = new MachineDataProcessor();
            var items = new[]
            {
                new MachineDataItem("Cappuccino", new DateTime(2023,08,25,8,0,0)),
                new MachineDataItem("Cappuccino", new DateTime(2023,08,25,9,0,0)),
                new MachineDataItem("Espresso", new DateTime(2023,08,25,10,0,0)),
            };

            // Act
            machineDataProcessor.ProcessItems(items);

            // Assert 

        }
    }
}
