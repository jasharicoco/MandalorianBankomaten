namespace MandalorianBankomaten.Accounts
{
    internal class SavingAccount : Account
    {
        // Private fields
        private decimal _interestRate;

        // Public properties
        public decimal InterestRate
        {
            get => _interestRate;
            set
            {
                //if (value < 0)
                //throw new ArgumentException("Interest rate cannot be negative.");
                _interestRate = value;
            }
        }

        // Constructor
        public SavingAccount(string accountName, string currencyCode, decimal initialBalance, decimal interestRate)
            : base(accountName, currencyCode, initialBalance)
        {
            _interestRate = 0.05m; // Set interest to 5%
        }

        // Methods
        public void ApplyInterest()
        {
            decimal totalInterest = Balance * _interestRate;
            Balance += totalInterest;
            Console.WriteLine($"Räntesats: {_interestRate:P}\nÅrlig ränta: {totalInterest:C}\nSaldo inklusive ränta: {CurrencyConverter.FormatAmount(Balance, CurrencyCode)}");
        }
        public override string ToString()
        {
            return $"Account Name: {AccountName}, Balance: {CurrencyConverter.FormatAmount(Balance, CurrencyCode)}, Account ID: {AccountID}, Interest Rate: {InterestRate}";
        }

    }
}
