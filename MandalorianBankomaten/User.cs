namespace MandalorianBankomaten;

public class User
{
    private string _name;
    private string _password;
    private List<Account> _accounts = new List<Account>();
    public string Name { get { return _name; } set; }
    public string Password { get { return _password; } set; }
    public List<Account> Accounts { get { return _accounts; } set; } // En användare kan ha flera konton

    public User(string name, string password)
    {
        Name = name;
        Password = password;
        Accounts = new List<Account>();
    }
    
    public void ShowAccounts()
    {
        if (Accounts.Count == 0)
        {
            Console.WriteLine($"Användare: {Name} har inga konton.");
            return;
        }
        Console.WriteLine($"Konton för användare: {Name}");
        foreach (var account in Accounts)
        {
            Console.WriteLine($" - Konto: {account.AccountName}, Saldo: {account.Balance}");
        }
    }

    public void AddAccount(Account account)
    {
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Kontot kan inte vara null.");
        }
        _accounts.Add(account);
    }

    public void RemoveAccount(Account account)
    {
        if (_accounts.Contains(account))
        {
            _accounts.Remove(account);
        }
        else
        {
            Console.WriteLine("Kontot finns inte i listan.");
        }
    }

    public void TransferMoneyBetweenAccounts(Account fromAccount, Account toAccount, decimal amount)
    {
        if (fromAccount == null || toAccount == null)
        {
            Console.WriteLine("Ett eller båda konton är ogiltiga.");
            return;
        }

        if (amount <= 0)
        {
            Console.WriteLine("Beloppet måste vara större än noll.");
            return;
        }

        if (fromAccount.Balance >= amount)
        {
            fromAccount.Withdraw(amount);
            toAccount.Deposit(amount);
            Console.WriteLine($"Överförde {amount:C} från {fromAccount.AccountName} till {toAccount.AccountName}.");
        }
        else
        {
            Console.WriteLine("Otillräckligt saldo för överföring.");
        }
    }

    public void TransferMoneyToUser(User recipient, Account fromAccount, Account recipientAccount, decimal amount)
    {
        if (recipient == null || fromAccount == null || recipientAccount == null)
        {
            Console.WriteLine("Mottagare eller något av kontona är ogiltiga.");
            return;
        }

        if (amount <= 0)
        {
            Console.WriteLine("Beloppet måste vara större än noll.");
            return;
        }

        if (fromAccount.Balance >= amount)
        {
            fromAccount.Withdraw(amount);
            recipientAccount.Deposit(amount);
            Console.WriteLine($"Överförde {amount:C} från {fromAccount.AccountName} till {recipient.Name}'s {recipientAccount.AccountName}.");
        }
        else
        {
            Console.WriteLine("Otillräckligt saldo för överföring.");
        }
    }
}