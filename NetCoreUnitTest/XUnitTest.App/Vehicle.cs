namespace XUnitTest.App
{
    /// <summary>
    /// Araç Sınıfı
    /// </summary>
    public class Vehicle
    {
        /// <summary>
        /// Kat edilen mesafeye göre yakıt tüketimini hesapla
        /// </summary>
        /// <param name="distance">Mesafe KM</param>
        /// <param name="fuelPrice">Yakıt Değeri</param>
        /// <returns></returns>
        public int CalculateConsumptionByDistance(int distance, int fuelPrice)
        {
            return (distance * fuelPrice) / 5;
        }
    }
}
