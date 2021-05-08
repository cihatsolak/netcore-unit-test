﻿using Xunit;
using XUnitTest.App;

namespace XUnitTest.Test.App
{
    /// <summary>
    /// Vehicle Test
    /// </summary>
    public class VehicleTest
    {
        public Vehicle Vehicle { get; set; }
        public VehicleTest()
        {
            Vehicle = new Vehicle();
        }

        [Fact]
        public void CalculateConsumptionByDistanceTestFact()
        {
            //Arrage Evresi
            int distance = 5;
            int fuelPrice = 20;

            //Act Evresi
            int consumption = Vehicle.CalculateConsumptionByDistance(distance, fuelPrice);

            //Assert ->  Assert.Equal(beklenen değer, gerçek değer);
            Assert.Equal(20, consumption);
        }

        [Theory]
        [InlineData(5, 20)]
        public void CalculateConsumptionByDistanceTestTheory(int distance, int fuelPrice)
        {
            //Arrage Evresi
            //constructor'da  tanımlandı

            //Act Evresi
            int consumption = Vehicle.CalculateConsumptionByDistance(distance, fuelPrice);

            //Assert ->  Assert.Equal(beklenen değer, gerçek değer);
            Assert.Equal<int>(20, consumption);
            Assert.InRange<int>(consumption, 15, 30);
            Assert.NotEqual<int>(0, consumption);
        }

        [Theory]
        [InlineData(5, 20)]
        public void CalculateConsumptionByDistance_SimpleValues_ReturnCalculateConsumption(int distance, int fuelPrice)
        {
            //Arrage Evresi
            //constructor'da  tanımlandı

            //Act Evresi
            int consumption = Vehicle.CalculateConsumptionByDistance(distance, fuelPrice);

            //Assert ->  Assert.Equal(beklenen değer, gerçek değer);
            Assert.Equal<int>(20, consumption);
            Assert.InRange<int>(consumption, 15, 30);
            Assert.NotEqual<int>(0, consumption);
        }

        [Theory]
        [InlineData(5, 0)]
        [InlineData(0, 20)]
        public void CalculateConsumptionByDistance_ZeroValues_ReturnZeroValue(int distance, int fuelPrice)
        {
            //Arrage Evresi
            //constructor'da  tanımlandı

            //Act Evresi
            int consumption = Vehicle.CalculateConsumptionByDistance(distance, fuelPrice);

            //Assert ->  Assert.Equal(beklenen değer, gerçek değer);
            Assert.Equal<int>(0, consumption);
        }
    }
}
