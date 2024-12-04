using System.Globalization;

namespace MandalorianBankomaten
{
    public static class CurrencyConverter
    {
        private static Dictionary<string, (decimal Rate, string Culture)> CurrencyData = new()
        {
            //counted from the swedish currency
            {"SEK", (1.00m, "en-SV") },
            {"USD", (0.091m, "en-US")},
            {"EUR", (0.087m, "en-EU")},
            {"DKK", (0.065m, "en-DK")},
            {"JPY", (13.98m, "en-JP")},
            {"GBP", (0.072m, "en-GB")}
        };


        //validates if the currency code exists 
        public static bool ValidateCurrencyCode(string fromCurrency, string toCurrency)
        {
            if (!CurrencyData.ContainsKey(fromCurrency) || !CurrencyData.ContainsKey(toCurrency))
            {
                return false;
            }
            else if (fromCurrency == toCurrency)
            {
                return false;
            }

            return true;
        }

        //calculates the amount and rate
        public static decimal Converter(string fromCurrency, string toCurrency, decimal amount)
        {
            decimal fromRate = CurrencyData[fromCurrency].Rate;
            decimal toRate = CurrencyData[toCurrency].Rate;

            decimal convertedAmount = amount * (toRate / fromRate);

            return convertedAmount;
        }

        public static decimal CurrencyConversion(string fromCurrency, string toCurrency, decimal amount)
        {
            if(ValidateCurrencyCode(fromCurrency, toCurrency))
            {
                return Converter(fromCurrency, toCurrency, amount);
            }

            return amount;
        }

        //can be used for new accounts
        public static string FormatAmount(string currencyCode, decimal amount)
        {
            if (CurrencyData.ContainsKey(currencyCode))
            {
                Console.WriteLine($"Invalid currency code: {currencyCode}");
            }

            var culture = new CultureInfo(CurrencyData[currencyCode].Culture);
            return amount.ToString("C", culture);
        }
    }
}
