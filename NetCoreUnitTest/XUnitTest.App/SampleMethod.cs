﻿using System.Collections.Generic;

namespace XUnitTest.App
{
    /// <summary>
    /// Asserts Metotlarını test etmek amacıyla oluşturulmuş örnek metotlar
    /// </summary>
    public class SampleMethod
    {
        public static string GetBrand()
        {
            return "Volkswagen";
        }

        public static List<string> GetModels()
        {
            return new List<string> { "Polo", "Golf", "Passat", "Passat" };
        }

        public static int GetVehicleYear()
        {
            return 2015;
        }
    }
}
