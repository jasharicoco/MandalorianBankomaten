using MandalorianBankomaten.Menu;

namespace MandalorianBankomaten.Accounts
{
    internal class Account
    {
        // Private fields
        private string _accountName;
        private decimal _balance;
        private string _currencyCode;
        private int _accountID;

        // Static counter for generating unique account IDs
        private static int _accountCounter = 4850;

        // Public properties
        public string AccountName
        {
            get => _accountName;
            private set
            {
                //if (string.IsNullOrWhiteSpace(value))
                //throw new ArgumentException("Account name cannot be null or empty.");
                _accountName = value;
            }
        }
        public decimal Balance
        {
            get => _balance;
            set
            {
                //if (value < 0)
                //throw new ArgumentException("Balance cannot be negative.");
                _balance = value;
            }
        }
        public string CurrencyCode
        {
            get => _currencyCode;
            private set
            {
                //if (string.IsNullOrWhiteSpace(value))
                //throw new ArgumentException("Currency code cannot be null or empty.");
                _currencyCode = value.ToUpper(); // Ensure uppercase for currency codes.
            }
        }
        public int AccountID
        {
            get => _accountID;
            private set
            {
                _accountID = value;
            }
        }

        // Constructor
        public Account(string accountName, string currencyCode, decimal initialBalance)
        {

            AccountID = _accountCounter; // Generate a unique account ID
            AccountName = accountName;
            CurrencyCode = currencyCode;
            Balance = initialBalance;
            _accountCounter++;
        }

        // Methods
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                MenuUtility.CustomWriteLine(49, "Insättningsbeloppet måste vara större än noll.");
                return;
            }

            Balance += amount;
            MenuUtility.CustomWriteLine(49, $"Insättning av {CurrencyConverter.FormatAmount(amount, CurrencyCode)} till {AccountName}.");
            MenuUtility.CustomWriteLine(49, $"Nytt saldo: {CurrencyConverter.FormatAmount(Balance, CurrencyCode)}.");
        }
        public void DepositSecret(decimal amount)
        {
            if (amount <= 0)
            {
                MenuUtility.CustomWriteLine(49, "Insättningsbeloppet måste vara större än noll.");
                return;
            }

            Balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Uttagsbeloppet måste vara större än noll.");
                return;
            }

            if (Balance >= amount)
            {
                Balance -= amount;

                MenuUtility.CustomWriteLine(49, $"Uttag av {CurrencyConverter.FormatAmount(amount, CurrencyCode)} från {AccountName}.");
                MenuUtility.CustomWriteLine(49, $"Nytt saldo: {CurrencyConverter.FormatAmount(Balance, CurrencyCode)}.");
            }
        }

        public override string ToString()
        {
            return $"Account Name: {AccountName}, Balance: {CurrencyConverter.FormatAmount(Balance, CurrencyCode)}, Account ID: {AccountID}";
        }

    }
}
