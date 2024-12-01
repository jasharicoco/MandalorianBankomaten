namespace MandalorianBankomaten
{
    public class CurrencyInformation : ICurrencyDataProvider
    {
        //properties to store in
        public decimal Amount { get; private set; }
        public string FromCurrency { get; private set; }
        public string ToCurrency { get; set; }

        public CurrencyInformation(decimal amount, string fromCurrency, string toCurrency)
        {
            FromCurrency = fromCurrency;
            ToCurrency = toCurrency;
            Amount = amount;
        }

        //=> is equal to {return ;} and this is a tuple
        public Dictionary<string, (decimal Rate, string Culture)> CurrencyData() => new()
        {
            //counted from the swedish currency
            {"SEK", (1.00m, "en-SV") },
            {"USD", (0.091m, "en-US")},
            {"EUR", (0.087m, "en-EU")},
            {"DKK", (0.065m, "en-DK")},
            {"JPY", (13.98m, "en-JP")},
            {"GBP", (0.072m, "en-GB")}
        };
    }
}
