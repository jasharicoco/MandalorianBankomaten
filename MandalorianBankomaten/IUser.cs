namespace MandalorianBankomaten;

public interface IUser // Interface for User
{
    int UserId { get; }
    string Name { get; }
    string Password { get; }
    List<Account> Accounts { get; }

    void ShowAccounts(); // shows all accounts
    void CreateAccount(); // creates an account
    void AddAccount(Account account); // adds an account
    void RemoveAccount(Account account); // removes an account
    Task TransferMoneyBetweenAccounts(Account fromAccount, Account toAccount, decimal amount); // transfers money between accounts
    Task TransferMoneyToUser(User recipient, Account fromAccount, Account recipientAccount, decimal amount); // transfers money to another user
}