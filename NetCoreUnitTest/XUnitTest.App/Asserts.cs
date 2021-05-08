using System.Collections.Generic;

namespace XUnitTest.App
{
    /// <summary>
    /// Asserts Metotlarını test etmek amacıyla oluşturulmuş örnek metotlar
    /// </summary>
    public class Asserts
    {
        public string GetBrand()
        {
            return "Volkswagen";
        }

        public List<string> GetModels()
        {
            return new List<string> { "Polo", "Golf", "Passat" };
        }
    }
}
