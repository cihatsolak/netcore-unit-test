using Moq;
using Xunit;
using XUnitTest.App;
using XUnitTest.App.Interfaces;

namespace XUnitTest.Test.App
{
    public class VehicleCreditTest
    {
        //VehicleCredit class'ının içerisindeli ICreditService interface'ini taklit ediyorum.
        public Mock<ICreditService> CreditServiceMock { get; set; }
        //Ana class
        public VehicleCredit VehicleCredit { get; set; }

        public VehicleCreditTest()
        {
            CreditServiceMock = new Mock<ICreditService>();
            //Ana class'ın ihtiyaç duyduğu ICreditService interface'ini verdim.
            VehicleCredit = new VehicleCredit(CreditServiceMock.Object);
        }

        /// <summary>
        /// Araç kredi tutarını belirlediğim değerlerle test ediyorum
        /// </summary>
        /// <param name="brand">Marka</param>
        /// <param name="modelYear">Model Yıl</param>
        /// <param name="expectedCreditAmount">Beklenen kredi tutarı</param>
        [Theory]
        [InlineData("Volkswagen", 2021, 200)]
        [InlineData("Seat", 2020, 185)]
        public void Add_SimpleValues_ReturnCreditAmount(string brand, int modelYear, int expectedCreditAmount)
        {
            //Eğer ICreditService içerisinde GetVehicleCreditAmount metotu çağrılırsa return olarak expectedCreditAmount değerini dön.
            CreditServiceMock.Setup(p => p.GetVehicleCreditAmount(brand, modelYear)).Returns(expectedCreditAmount);

            int creditAmount = VehicleCredit.Calculate(brand, modelYear);

            //GetVehicleCreditAmount servisinden 200 dönecek(Mock), Calculate buna 100 ekleyecek ve gelen değer 300 olacak.
            //Seat için 185 + 100 = 285 dönecek
            //Volkswagen için 200 + 100 = 300 dönecek
            Assert.InRange(creditAmount, 280, 310);
        }
    }
}
