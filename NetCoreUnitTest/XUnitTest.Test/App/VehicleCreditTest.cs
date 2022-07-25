using Moq;
using System;
using System.Threading;
using Xunit;
using XUnitTest.App;
using XUnitTest.App.Interfaces;

namespace XUnitTest.Test.App
{
    /// <summary>
    /// Moq Framework ile test işlemleri
    /// </summary>
    public class VehicleCreditTest
    {
        //VehicleCredit class'ının içerisindeli ICreditService interface'ini taklit ediyorum.
        public Mock<ICreditService> CreditServiceMock { get; set; }
        
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

        /// <summary>
        /// Mock - Throws
        /// </summary>
        /// <param name="brand">Marka</param>
        /// <param name="modelYear">Model Yıl</param>
        /// <param name="expectedCreditAmount">Beklenen kredi tutarı</param>
        [Theory]
        [InlineData("Seat", 0)]
        [InlineData("Volkswagen Ticari", 0)]
        public void CreditAmount_ZeroModelYearValue_ReturnArgumentNullException(string brand, int modelYear)
        {
            //Eğer ICreditService içerisinde GetVehicleCreditAmount metotu çağrılırsa ve model yılı 0 ve altında ise return olarak ArgumentNullException fırlat.
            CreditServiceMock.Setup(p => p.GetVehicleCreditAmount(brand, modelYear)).Throws(new ArgumentNullException(nameof(modelYear), "model year cannot be less than zero"));

            //GetVehicleCreditAmount servisine model yılı 0 ve altında bir değer girildiğinde ArgumentNullException fırlatılacak.
            var argumentNullException = Assert.Throws<ArgumentNullException>(() => VehicleCredit.CreditAmount(brand, modelYear));
            //Dönen hata mesajı doğru mu?
            Assert.Equal("model year cannot be less than zero (Parameter 'modelYear')", argumentNullException.Message);
        }

        /// <summary>
        /// Mock - Callback
        /// </summary>
        /// <param name="brand">Marka</param>
        /// <param name="modelYear">Model Yıl</param>
        /// <param name="expectedCreditAmount">Beklenen kredi tutarı</param>
        [Theory]
        [InlineData("Volkswagen", 2021)]
        public void CreditAmount_BrandAndModelYearValues_ReturnCreditAmountValue(string brand, int modelYear)
        {
            int actualCreditAmount = 0;

            //Eğer ICreditService içerisinde GetVehicleCreditAmount metotu çağrılırsa return olarak expectedCreditAmount değerini dön.
            CreditServiceMock.Setup(p => p.GetVehicleCreditAmount(It.IsAny<string>(), It.IsAny<int>())).Callback<string, int>((x, y) => actualCreditAmount = y / 3);

            VehicleCredit.CreditAmount(brand, modelYear);
            Assert.Equal(673, actualCreditAmount);

            VehicleCredit.CreditAmount("Seat", 2012);
            Assert.Equal(670, actualCreditAmount);
        }
    }
}
