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
