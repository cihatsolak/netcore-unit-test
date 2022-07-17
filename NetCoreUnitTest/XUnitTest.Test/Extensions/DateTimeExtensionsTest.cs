using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;
using XUnitTest.App.Extensions;
using XUnitTest.Test.Poco;

namespace XUnitTest.Test.Extensions
{
    public class DateTimeExtensionsTest
    {
        [Fact]
        public void ToPrettyDate_ShouldArgumentNullException_WhenCultureIsNull()
        {
            var result = Record.Exception(() => DateTime.Now.ToPrettyDate(null));
            Assert.NotNull(result);
            var exception = Assert.IsType<ArgumentNullException>(result);
            var actual = exception.ParamName;
            const string expected = "culture";
            Assert.Equal(expected, actual);
        }
        
    }
}
