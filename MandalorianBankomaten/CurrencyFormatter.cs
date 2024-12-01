using System.Globalization;

namespace MandalorianBankomaten
{
    public class CurrencyFormatter : ICurrencyFormatter
    {
        private readonly ICurrencyDataProvider _dataProvider;

        //to take the data from the dictioanry 
        public CurrencyFormatter(ICurrencyDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public string FormatAmount(decimal amount, string currencyCode)
        {
            var data = _dataProvider.CurrencyData();
            if (!data.ContainsKey(currencyCode))
            {
                throw new ArgumentException($"Invalid currency code: {currencyCode}");
            }

            var culture = new CultureInfo(data[currencyCode].Culture);
            return amount.ToString("C", culture);
        }
    }
}
