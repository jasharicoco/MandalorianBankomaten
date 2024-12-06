using System.Globalization;
using System.Collections.Generic;

namespace MandalorianBankomaten
{
    //programmed by margo
    public static class CurrencyConverter
    {

        private static Dictionary<string, (decimal Rate, CultureInfo Culture)> CurrencyData = new()
        {

             {"SEK", (1.00m, new CultureInfo("sv-SE")) },
             {"USD", (0.091m, new CultureInfo("en-US")) },
             {"EUR", (0.087m, new CultureInfo("en-EU")) },
             {"DKK", (0.065m, new CultureInfo("da-DK")) },
             {"JPY", (13.98m, new CultureInfo("en-JP")) },
             {"GBP", (0.072m, new CultureInfo("en-GB")) }

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

        //method for both the converter and valdiation 
        public static decimal CurrencyConversion(string fromCurrency, string toCurrency, decimal amount)
        {
            if (ValidateCurrencyCode(fromCurrency, toCurrency))
            {
                return Converter(fromCurrency, toCurrency, amount);
            }

            return amount;
        }

        //can be used for new accounts
        public static string FormatAmount(decimal amount, string currencyCode)
        {
            if (!CurrencyData.ContainsKey(currencyCode))
            {
                Console.WriteLine($"Invalid currency code: {currencyCode}");
            }

            CultureInfo culture = (CurrencyData[currencyCode].Culture);

            return amount.ToString("C", culture);
        }
    }
}
