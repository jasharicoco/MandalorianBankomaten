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
            InterestRate = interestRate;
        }

        // Methods
        public void ApplyInterest() { }
        public override string ToString()
        {
            return $"Account Name: {AccountName}, Balance: {Balance.ToString("C")}, Currency: {CurrencyCode}, Account ID: {AccountID}, Interest Rate: {InterestRate}";
        }

    }
}
