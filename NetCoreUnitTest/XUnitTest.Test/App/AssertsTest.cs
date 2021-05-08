using System;
using System.Collections.Generic;
using Xunit;
using XUnitTest.App;

namespace XUnitTest.Test.App
{
    /// <summary>
    /// Asserts Metotlarını test etmek 
    /// </summary>
    public class AssertsTest
    {
        /// <summary>
        /// Contain / DoesNotContain
        /// </summary>
        [Fact]
        public void ContainOrDoesNotContains()
        {
            var assert = new Asserts();

            string brand = assert.GetBrand();
            List<string> models = assert.GetModels();

            Assert.Contains("agen", brand);
            Assert.DoesNotContain("skoda", brand);

            Assert.Contains(models, item => item.Equals("Ateca", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// True / False
        /// </summary>
        [Fact]
        public void TrueOrFalse()
        {
            var assert = new Asserts();
            string brand = assert.GetBrand();

            Assert.True(brand.GetType() == typeof(string));
            Assert.False(brand.GetType() == typeof(int));
        }

        /// <summary>
        /// Match / DoesNotMatch
        /// </summary>
        [Fact]
        public void MatchOrDoesNotMatch()
        {
            var assert = new Asserts();
            string brand = assert.GetBrand();

            Assert.Matches("^Volk", brand);
            Assert.DoesNotMatch("dog$", brand);
        }

        /// <summary>
        /// StartWith / EndWith
        /// </summary>
        [Fact]
        public void StartWithOrEndWith()
        {
            var assert = new Asserts();
            string brand = assert.GetBrand();

            Assert.StartsWith("Volks", brand);
            Assert.EndsWith("agen", brand);
        }
    }
}
