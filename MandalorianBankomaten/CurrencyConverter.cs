using MandalorianBankomaten.Menu;
using System.Globalization;

namespace MandalorianBankomaten
{
    //programmed by margo
    public static class CurrencyConverter
    {

        private static Dictionary<string, (decimal Rate, CultureInfo Culture)> CurrencyData = new()
        {
             {"SEK", (1.00m, new CultureInfo("sv-SE")) }, //basvaluta
             {"USD", (0.091m, new CultureInfo("en-US")) },
             {"EUR", (0.087m, new CultureInfo("en-EU")) },
             {"DKK", (0.065m, new CultureInfo("da-DK")) },
             {"JPY", (13.98m, new CultureInfo("en-JP")) },
             {"GBP", (0.072m, new CultureInfo("en-GB")) }
        };


        //validates if the currency code exists 
        public static bool ValidateCurrency(string fromCurrency, string toCurrency)
        {
            if (!CurrencyData.ContainsKey(fromCurrency) || !CurrencyData.ContainsKey(toCurrency))
            {
                return false;
            }

            return true;
        }

        //calculates the amount and rate
        public static decimal Converter(string fromCurrency, string toCurrency, decimal amount)
        {
            // Om valutorna är samma, ingen konvertering behövs
            if (fromCurrency == toCurrency)
            {
                return amount;
            }

            // Hämta växelkurser
            decimal fromRate = CurrencyData[fromCurrency].Rate;
            decimal toRate = CurrencyData[toCurrency].Rate;

            // Beräkna det omvandlade beloppet
            decimal convertedAmount = amount * (toRate / fromRate);

            return convertedAmount; // Detta är beloppet i "toCurrency"
        }

        //method for both the converter and valdiation 
        public static decimal? CurrencyConversion(string fromCurrency, string toCurrency, decimal amount)
        {
            if (!ValidateCurrency(fromCurrency, toCurrency))
            {
                return null;
            }

            decimal convertedAmount = Converter(fromCurrency, toCurrency, amount);
            return convertedAmount;
        }

        //can be used for new accounts
        public static string FormatAmount(decimal amount, string currencyCode)
        {
            if (!CurrencyData.ContainsKey(currencyCode))
            {
                MenuUtility.CustomWriteLine(49, $"Invalid currency code: {currencyCode}");
                return null;
            }

            CultureInfo culture = (CurrencyData[currencyCode].Culture);

            return amount.ToString("C", culture);
        }
    }
}