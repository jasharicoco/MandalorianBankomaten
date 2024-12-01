namespace MandalorianBankomaten
{
    public interface ICurrencyConverter
    {
        decimal Convert(decimal amount, string fromCurrency, string toCurrency);
    }
}
