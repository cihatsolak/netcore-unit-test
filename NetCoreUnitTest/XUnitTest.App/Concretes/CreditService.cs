using System;
using XUnitTest.App.Interfaces;
using XUnitTest.App.Models;

namespace XUnitTest.App.Concretes
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
            if (string.IsNullOrEmpty(brand))
                throw new ArgumentNullException(nameof(brand), "brand cannot be empty");

            if (0 >= modelYear)
                throw new ArgumentNullException(nameof(modelYear), "model year cannot be less than zero");

            if (brand.Equals("Volkswagen") && modelYear == 2021)
                return 200;
            else if (brand.Equals("Seat") && modelYear == 2020)
                return 185;
            else
                return 150;
        }

        /// <summary>
        /// Model, yıl ve yakıt tipine göre kaç taksit olacağını hesaplar
        /// </summary>
        /// <param name="model">Model</param>
        /// <param name="modelYear">Model yılı</param>
        /// <param name="fuel">Yakıt tipi</param>
        /// <returns>Taksit sayısı</returns>
        public int CalculateInstallments(string model, int modelYear, string fuel)
        {
            if (model.Equals("Polo") && modelYear == 2021 && fuel.Equals("Benzin"))
                return 10;
            else if (model.Equals("Octavia") && modelYear == 2015 && fuel.Equals("Dizel"))
                return 8;
            else
                return 12;
        }

        public bool QuestionTheTrafficTicket(TrafficTicketDto trafficTicketDto)
        {
            if (string.IsNullOrWhiteSpace(trafficTicketDto.Plate))
                return false;

            if (0 >= trafficTicketDto.Age)
                return false;

            return true;
        }
    }
}
