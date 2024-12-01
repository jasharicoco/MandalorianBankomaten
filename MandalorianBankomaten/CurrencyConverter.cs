using System.Globalization;

namespace MandalorianBankomaten
{
    public class CurrencyConverter : ICurrencyConverter
    {
        private readonly ICurrencyDataProvider _dataProvider;
        private readonly ICurrencyValidator _validator;

        public CurrencyConverter(ICurrencyDataProvider dataProvider, ICurrencyValidator validator)
        {
            _dataProvider = dataProvider;
            _validator = validator;
        }

        public decimal Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            _validator.ValidateCode(fromCurrency);
            _validator.ValidateCode(toCurrency);

            //gets the data from the dictionary 
            var data = _dataProvider.CurrencyData();
            //takes the value1 - rate
            var fromRate = data[fromCurrency].Rate;
            var toRate = data[toCurrency].Rate;

            //can exchange every currency
            return amount * (fromRate / toRate);
        }
    }
}
