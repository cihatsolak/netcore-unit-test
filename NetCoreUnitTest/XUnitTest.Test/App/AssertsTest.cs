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

        }
    }
}
