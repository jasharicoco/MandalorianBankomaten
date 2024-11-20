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
        if (account == null)
        {
            throw new ArgumentNullException(nameof(account), "Kontot kan inte vara null.");
        }
        if (_accounts.Contains(account))
        {
            _accounts.Remove(account);
        }
        else
        {
            Console.WriteLine("Kontot finns inte i listan.");
        }
    }
}