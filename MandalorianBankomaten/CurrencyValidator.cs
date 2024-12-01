namespace MandalorianBankomaten
{
    public class CurrencyValidator : ICurrencyValidator
    {
        private readonly ICurrencyDataProvider _dataProvider;

        //constuctor to store the currency data fron the dictionaryn
        public CurrencyValidator(ICurrencyDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public void ValidateCode(string currencyCode)
        {
            //checks if chosen currency exists
            if (!_dataProvider.CurrencyData().ContainsKey(currencyCode))
            {
                throw new ArgumentException($"Invaild currency code: {currencyCode}");
            }
        }
    }
}
