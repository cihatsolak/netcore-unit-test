using XUnitTest.App.Models;

namespace XUnitTest.App.Interfaces
{
    /// <summary>
    /// Araç servisi, DI
    /// </summary>
    public interface ICreditService
    {
        /// <summary>
        /// Marka ve model yılına göre kredi tutarını alın
        /// </summary>
        /// <param name="brand">Marka</param>
        /// <param name="modelYear">Model yılı</param>
        /// <returns>Kredi tutarı</returns>
        int GetVehicleCreditAmount(string brand, int modelYear);

        /// <summary>
        /// Model, yıl ve yakıt tipine göre kaç taksit olacağını hesaplar
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="modelYear">Model yılı</param>
        /// <param name="fuel">Yakıt tipi</param>
        /// <returns>Taksit sayısı</returns>
        int CalculateInstallments(string model, int modelYear, string fuel);

        /// <summary>
        /// Trafik cezasını sorgular
        /// </summary>
        /// <returns></returns>
        bool QuestionTheTrafficTicket(TrafficTicketDto trafficTicketDto);
    }
}
