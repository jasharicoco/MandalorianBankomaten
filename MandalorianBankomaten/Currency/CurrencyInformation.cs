using System.Globalization;

namespace MandalorianBankomaten.Currency
{
    internal class CurrencyInformation
    {
        // Private fields
        private decimal _amount;
        private string _fromCurrency;
        private string _toCurrency;
        private string _currencyCode;

        // Public properties
        public decimal Amount
        {
            get { return _amount; }
            set
            {
                if (_amount > 50000m)
                {
                    throw new ArgumentOutOfRangeException("The amount is to high, contact the admin.");
                }
            }
        }
        public string FromCurrency
        {
            get { return _fromCurrency; }
        }
        public string ToCurrency
        {
            get { return _toCurrency; }
        }
        public string CurrencyCode
        {
            get { return _currencyCode; }
            //set { CurrencyData.ContainsKey(CurrencyCode); }
        }

        // Constructor
        public CurrencyInformation(decimal Amount, string FromCurrency, string ToCurrency, string CurrencyCode)
        {
            _fromCurrency = FromCurrency;
            _toCurrency = ToCurrency;
            _amount = Amount;
            _currencyCode = CurrencyCode;
        }

        // Dictionary to store exchange rates
        private Dictionary<string, (decimal Rate, string Culture)> CurrencyData = new()
        {
            //counted from the swedish currency
            {"SEK", (1.00m, "en-SV") },
            {"USD", (0.091m, "en-US")},
            {"EUR", (0.087m, "en-EU")},
            {"DKK", (0.065m, "en-DK")},
            {"JPY", (13.98m, "en-JP")},
            {"GBP", (0.072m, "en-GB")}
        };

        // Methods
        //validates if the currency code exists 
        public void ValidateCurrencyCode()
        {
            if (!CurrencyData.ContainsKey(FromCurrency) || !CurrencyData.ContainsKey(ToCurrency))
            {
                throw new ArgumentException($"Invaild currency code: {CurrencyCode}");
            }
            else if (CurrencyData.ContainsKey(FromCurrency) == CurrencyData.ContainsKey(ToCurrency))
            {
                throw new ArgumentException("Cannot exchange with the same currency");
            }

            if (CurrencyData.ContainsKey(FromCurrency) && CurrencyData.ContainsKey(ToCurrency))
            {
                Converter();
            }
        }

        //calculates the amount and rate
        public string Converter()
        {
            decimal fromRate = CurrencyData[FromCurrency].Rate;
            decimal toRate = CurrencyData[ToCurrency].Rate;

            var culture = new CultureInfo(CurrencyData[CurrencyCode].Culture);
            decimal convertedAmount = Amount * (toRate / fromRate);

            string converter = convertedAmount.ToString("C", culture);
            return converter;
        }

        //can be used for new accounts 
        public string FormatAmount()
        {
            if (CurrencyData.ContainsKey(CurrencyCode))
            {
                throw new ArgumentException($"Invalid currency code: {CurrencyCode}");
            }

            var culture = new CultureInfo(CurrencyData[CurrencyCode].Culture);
            return Amount.ToString("C", culture);
        }

    }
}