namespace MandalorianBankomaten;

public class User
{
    public string Name { get; set; }
    public string Password { get; set; }
    public List<Account> Accounts { get; set; } // En användare kan ha flera konton

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
            Console.WriteLine($" - Konto: {account.AccountName}, Saldo: {account.AccountMoney}");
        }
    }
}