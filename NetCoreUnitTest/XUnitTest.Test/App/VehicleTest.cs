using Xunit;
using XUnitTest.App;

namespace XUnitTest.Test.App
{
    /// <summary>
    /// Vehicle Test
    /// </summary>
    public class VehicleTest
    {
        [Fact]
        public void CalculateConsumptionByDistanceTest()
        {
            //Arrage Evresi
            int distance = 5;
            int fuelPrice = 20;
            var vehicle = new Vehicle();

            //Act Evresi
            int consumption = vehicle.CalculateConsumptionByDistance(distance, fuelPrice);

            //Assert ->  Assert.Equal(beklenen değer, gerçek değer);
            Assert.Equal(20, consumption);
        }
    }
}
