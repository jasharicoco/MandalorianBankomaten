namespace MandalorianBankomaten;

public interface IUser // Interface for User
{
    int UserId { get; }
    string Name { get; }
    string Password { get; }
    List<Account> Accounts { get; }

}