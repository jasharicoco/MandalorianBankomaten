using System.Globalization;

namespace MandalorianBankomaten
{
    public class Account : IAccount
    {
        private int _accountID;
        private string _accountName;
        private decimal _balance;
        private string _currencyCode;
        public string AccountName { get; private set; }
        public decimal Balance { get; set; }
        public string CurrencyCode { get; private set; }
        public int AccountID { get; private set; }

        private static int accountCounter = 4850;


        public Account(string accountName, decimal balance, string currencyCode)
        {
            accountCounter++;
            AccountName = accountName;
            Balance = balance;
            CurrencyCode = currencyCode;
            AccountID = accountCounter;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
            {
                Console.WriteLine("Insättningsbeloppet måste vara större än noll.");
                return;
            }

            Balance += amount;
            Console.WriteLine($"Insättning av {amount.ToString("C", CultureInfo.CurrentCulture)} till {AccountName}. Nytt saldo: {Balance.ToString("C", CultureInfo.CurrentCulture)}.");
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
                Console.WriteLine($"Uttag av {amount.ToString("C", CultureInfo.CurrentCulture)} från {AccountName}. Nytt saldo: {Balance.ToString("C", CultureInfo.CurrentCulture)}.");
            }
            else
            {
                Console.WriteLine("Otillräckligt saldo för uttag.");
            }
        }

        public void DisplayAccountDetails()
        {
            Console.WriteLine($"Kontonamn: {AccountName}, Saldo: {Balance.ToString("C")}, Valuta: {CurrencyCode}, Konto-ID: {AccountID}");
        }
    }
}