using System.Xml.Linq;

namespace MandalorianBankomaten;

public class Account
{
    private string _accountName;
    private decimal _balance;
    private string _currency;
    public string AccountName { get { return _accountName; } set; }
    public decimal Balance { get { return _balance; } set; }
    public string Currency { get { return _currency; } set; }

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
        Console.WriteLine($"Insättning av {amount:C} till {AccountName}. Nytt saldo: {Balance:C}.");
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
            Console.WriteLine($"Uttag av {amount:C} från {AccountName}. Nytt saldo: {Balance:C}.");
        }
        else
        {
            Console.WriteLine("Otillräckligt saldo för uttag.");
        }
    }


}