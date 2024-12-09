using MandalorianBankomaten.Menu;

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
            MenuUtility.CustomWriteLine(49, $"Räntesats: {_interestRate:P} , {CurrencyCode}");
            MenuUtility.CustomWriteLine(49, $"Årlig ränta: { CurrencyConverter.FormatAmount(totalInterest, CurrencyCode)}");
            MenuUtility.CustomWriteLine(49, $"Saldo inklusive ränta: {CurrencyConverter.FormatAmount(Balance, CurrencyCode)}");
        }
        public override string ToString()
        {
            return $"Account Name: {AccountName} \nBalance: {CurrencyConverter.FormatAmount(Balance, CurrencyCode)} \nAccount ID: {AccountID} \nInterest Rate: {InterestRate}";
        }

    }
}
