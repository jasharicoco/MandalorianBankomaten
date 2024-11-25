using System.Reflection.Metadata;

namespace MandalorianBankomaten
{
    public class Admin : IUser // admin class implements IUser interface
{
    public string Name { get; private set; }
    public string Password { get; private set; }
    public int UserId { get; private set; }
    public List<Account> Accounts { get; private set; } = new List<Account>();

    static int _adminCounter = 0;

    public Admin(string name, string password)
    {
        _adminCounter++;
        Name = name;
        Password = password;
        UserId = _adminCounter;
    }

    public void ShowAccounts()
    {
        Console.WriteLine($"Admin {Name} visar alla användares konton...");
    }

    public void CreateAccount()
    {
        Console.WriteLine("Admin kan inte skapa ett konto för sig själv via denna metod.");
    }

    public void AddAccount(Account account)
    {
        Console.WriteLine("Admin kan lägga till konton för användare.");
    }

    public void RemoveAccount(Account account)
    {
        Console.WriteLine("Admin kan ta bort konton för användare.");
    }

    public async Task TransferMoneyBetweenAccounts(Account fromAccount, Account toAccount, decimal amount)
    {
        if (fromAccount.Balance >= amount)
        {
            Console.WriteLine("Förbereder överföring... Vänta 1 minut.");
            await Task.Delay(TimeSpan.FromMinutes(1));

            fromAccount.Withdraw(amount);
            toAccount.Deposit(amount);
            Console.WriteLine($"Överföring från {fromAccount.AccountName} till {toAccount.AccountName} lyckades.");
        }
        else
        {
            Console.WriteLine("Otillräckligt saldo för överföring.");
        }
    }

    public async Task TransferMoneyToUser(User recipient, Account fromAccount, Account recipientAccount, decimal amount)
    {
        if (fromAccount.Balance >= amount)
        {
            Console.WriteLine("Förbereder överföring... Vänta 1 minut.");
            await Task.Delay(TimeSpan.FromMinutes(1));

            fromAccount.Withdraw(amount);
            recipientAccount.Deposit(amount);
            Console.WriteLine($"Överföring från {fromAccount.AccountName} till {recipient.Name}'s {recipientAccount.AccountName} lyckades.");
        }
        else
        {
            Console.WriteLine("Otillräckligt saldo för överföring.");
        }
    }
    
    public List<User> CreateUser(List<User> users)
    {
        Console.Write("Vänligen skriv in ett användarnamn: ");
        string newUsername = Console.ReadLine().ToLower();

        bool usernameExists = users.Any(user => user.Name.ToLower() == newUsername);

        if (usernameExists)
        {
            Console.WriteLine("Användarnamnet är redan upptaget. Försök med ett annat.");
            return users;
        }

        Console.Write("Vänligen skriv in ett lösenord: ");
        string newPassword = Console.ReadLine();

        User newUser = new User(newUsername, newPassword);
        users.Add(newUser);
        Console.WriteLine("Användaren har skapats!");
        return users;
    }
}
}
