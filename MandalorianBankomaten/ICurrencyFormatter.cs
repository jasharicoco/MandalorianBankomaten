namespace MandalorianBankomaten
{
    public interface ICurrencyFormatter
    {
        string FormatAmount(decimal amount, string currencyCode);
    }
}
