namespace MandalorianBankomaten
{
    public class Currency
    {
        public decimal Balance { get; set; }
        public string CurrencyCode { get; set; } = "SEK";
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }

        public Currency(string currencyCode, decimal rate, decimal amount)
        {
            CurrencyCode = currencyCode;
            Rate = rate;
            Amount = amount;
        }
      
        public List<decimal> rate = new List<decimal>();
       
        public void Calculate()
        {
            var Currencies = new List<string> { "SEK", "EUR", "USD" };

            // Needs error handling
            string fromCurrency = Console.ReadLine();
            string toCurrency = Console.ReadLine();

            if(Currencies.Contains(CurrencyCode))
            {
                //exchange rate formula A*B=C
                decimal Result = Amount * Rate;
            }
        }

    }
 
}