namespace MandalorianBankomaten
{
    public interface ICurrencyValidator
    {
        void ValidateCode(string currencyCode);
    }
}
