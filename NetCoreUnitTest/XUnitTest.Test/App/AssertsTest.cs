using System;
using System.Collections.Generic;
using Xunit;
using XUnitTest.App;

namespace XUnitTest.Test.App
{
    /// <summary>
    /// Asserts metotlarını test etmek 
    /// </summary>
    public class AssertsTest
    {
        /// <summary>
        /// Contain / DoesNotContain
        /// </summary>
        [Fact]
        public void ContainOrDoesNotContains()
        {
            string brand = SampleMethod.GetBrand();
            List<string> models = SampleMethod.GetModels();

            Assert.Contains("agen", brand);
            Assert.DoesNotContain("skoda", brand);

            Assert.Contains(models, item => item.Equals("Golf", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// True / False
        /// </summary>
        [Fact]
        public void TrueOrFalse()
        {
            string brand = SampleMethod.GetBrand();

            Assert.True(brand.GetType() == typeof(string));
            Assert.False(brand.GetType() == typeof(int));
        }

        /// <summary>
        /// Match / DoesNotMatch
        /// </summary>
        [Fact]
        public void MatchOrDoesNotMatch()
        {
            string brand = SampleMethod.GetBrand();

            Assert.Matches("^Volk", brand);
            Assert.DoesNotMatch("dog$", brand);
        }

        /// <summary>
        /// StartWith / EndWith
        /// </summary>
        [Fact]
        public void StartWithOrEndWith()
        {
            string brand = SampleMethod.GetBrand();

            Assert.StartsWith("Volks", brand);
            Assert.EndsWith("agen", brand);
        }

        /// <summary>
        /// Empty / NotEmpty
        /// </summary>
        [Fact]
        public void EmptyOrNotEmpty()
        {
            List<string> models = SampleMethod.GetModels();

            Assert.NotEmpty(models);
            Assert.Empty(new List<int>());
        }

        /// <summary>
        /// Range / NotInRange
        /// </summary>
        [Fact]
        public void RangeOrNotInRange()
        {
            int year = SampleMethod.GetVehicleYear();

            Assert.InRange(year, 2000, 2021);
            Assert.NotInRange(year, 1995, 1999);
        }

        /// <summary>
        /// Single
        /// </summary>
        [Fact]
        public void Single()
        {
            List<string> models = SampleMethod.GetSingleModels();

            Assert.Single(models);
        }

        /// <summary>
        /// IsType / IsNotType
        /// </summary>
        [Fact]
        public void IsTypeOrIsNotType()
        {
            Assert.IsType<int>(1);
            Assert.IsType<string>("cihat");

            Assert.IsNotType<int>("cihat");
            Assert.IsNotType<float>(2);
        }

        /// <summary>
        /// IsAssignableFrom
        /// </summary>
        [Fact]
        public void IsAssignableFrom()
        {
            Assert.IsAssignableFrom<IEnumerable<string>>(new List<string>());
            Assert.IsAssignableFrom<object>(2015);
        }

        /// <summary>
        /// Null / NotNull
        /// </summary>
        [Fact]
        public void NullOrNotNull()
        {
            List<string> models = SampleMethod.GetModels();

            Assert.NotNull(models);
            Assert.Null(null);
        }

        /// <summary>
        /// Equal / NotEqual
        /// </summary>
        [Fact]
        public void EqualOrNotEqual()
        {
            string brand = SampleMethod.GetBrand();

            Assert.Equal("Volkswagen", brand);
            Assert.NotEqual("Scoda", brand);
        }
    }
}
