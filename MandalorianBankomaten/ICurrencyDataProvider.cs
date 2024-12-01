namespace MandalorianBankomaten
{
    public interface ICurrencyDataProvider
    {
        Dictionary<string, (decimal Rate, string Culture)> CurrencyData();
    }
}
