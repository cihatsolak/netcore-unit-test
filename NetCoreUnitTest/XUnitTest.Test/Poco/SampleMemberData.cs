using System;
using System.Collections.Generic;
using System.Globalization;

namespace XUnitTest.Test.Poco
{
    public static class SampleMemberData
    {
        public static IEnumerable<object[]> StaticParameter => new List<CultureTestParameter[]>
        {
            new CultureTestParameter[]
            {
                new CultureTestParameter
                {
                    Culture = CultureInfo.CreateSpecificCulture("it-IT"),
                    Actual = new DateTime(1988, 06, 05),
                    Expected = "05 giugno 1988"
                }
            }
        };
    }
}
