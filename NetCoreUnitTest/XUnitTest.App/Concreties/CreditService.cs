using XUnitTest.App.Interfaces;

namespace XUnitTest.App.Concreties
{
    public class CreditService : ICreditService
    {
        /// <summary>
        /// Marka ve model yılına göre kredi tutarını alın
        /// </summary>
        /// <param name="brand">Marka</param>
        /// <param name="modelYear">Model yılı</param>
        /// <returns>Kredi tutarı</returns>
        public int GetVehicleCreditAmount(string brand, int modelYear)
        {
            if (brand.Equals("Volkswagen") && modelYear == 2021)
                return 200;
            else if (brand.Equals("Seat") && modelYear == 2020)
                return 185;
            else
                return 150;
        }
    }
}
