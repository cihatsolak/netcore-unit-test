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
        /// Araç bilgilerine göre kredi tutarını hesaplayayan metotu test ediyorum
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

            int creditAmount = VehicleCredit.CreditAmount(brand, modelYear);

            //GetVehicleCreditAmount servisinden 200 dönecek(Mock), Calculate buna 100 ekleyecek ve gelen değer 300 olacak.
            //Seat için 185 + 100 = 285 dönecek
            //Volkswagen için 200 + 100 = 300 dönecek
            Assert.InRange(creditAmount, 280, 310);
        }

        /// <summary>
        /// Araç bilgilerine göre kredinin kaç taksit olacağını hesaplayan metotu test ediyorum.
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="modelYear">Model Yılı</param>
        /// <param name="fuel">Yakıt Tipi</param>
        [Theory]
        [InlineData("Polo", 2021, "Benzin")]
        public void Installment_VehicleValues_ReturnNumberOfCreditInstallments(string model, int modelYear, string fuel)
        {
            //Eğer ICreditService içerisinde CalculateInstallments metotu çağrılırsa return olarak 10 değerini dön.
            CreditServiceMock.Setup(p => p.CalculateInstallments(model, modelYear, fuel)).Returns(10);

            //VehicleCredit.Installment metotu servisten dönen değeri 2'ye bölüyor.
            int installmentsCount = VehicleCredit.Installment(model, modelYear, fuel);

            //Servisten dönen değeri 2'ye böldüğü için beklenen değeri 5 verdim.
            Assert.Equal<int>(5, installmentsCount); //(xUnit ile doğrula)
            CreditServiceMock.Verify(p => p.CalculateInstallments(model, modelYear, fuel), Times.Once); //Bu metot 1 kere çalışsın. Eğer 2 kere çalışırsa test başarısız olacak.
        }
    }
}
