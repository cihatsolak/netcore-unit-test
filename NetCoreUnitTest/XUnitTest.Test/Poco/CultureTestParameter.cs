using System;
using System.Globalization;

namespace XUnitTest.Test.Poco
{
    public class CultureTestParameter
    {
        public CultureInfo Culture { get; set; }
        public DateTime Actual { get; set; }
        public string Expected { get; set; }
    }
}
