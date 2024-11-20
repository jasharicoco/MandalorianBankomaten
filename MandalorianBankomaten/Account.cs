using System;
using System.Globalization;

namespace MandalorianBankomaten
{
    public class Account
    {
        private string _accountName;
        private decimal _balance;
        private string _currency;

        public string AccountName { get; private set; }
        public decimal Balance { get; private set; }
        public string Currency { get; private set; }

        public Account(string accountName, decimal balance, string currency)
        {
            AccountName = accountName;
            Balance = balance;
            Currency = currency;
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
    }
}