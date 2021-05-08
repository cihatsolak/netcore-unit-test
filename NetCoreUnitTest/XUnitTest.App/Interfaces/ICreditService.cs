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
    }
}
