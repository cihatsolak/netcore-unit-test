using System;
using XUnitTest.App.Interfaces;

namespace XUnitTest.App
{
    /// <summary>
    /// Mock örnekleri
    /// </summary>
    public class VehicleCredit
    {
        private readonly ICreditService _creditService;
        public VehicleCredit(ICreditService creditService)
        {
            _creditService = creditService;
        }

        public int CreditAmount(string brand, int modelYear)
        {
            if (string.IsNullOrEmpty(brand))
                throw new ArgumentNullException(nameof(brand), "brand cannot be empty");

            if (0 >= modelYear)
                throw new ArgumentException("model year cannot be less than zero", nameof(modelYear));

            int creditAmount = _creditService.GetVehicleCreditAmount(brand, modelYear);

            return creditAmount + 100;
        }

        public int Installment(string model, int modelYear, string fuel)
        {
            int installment = _creditService.CalculateInstallments(model, modelYear, fuel);
            return installment / 2;
        }
    }
}
