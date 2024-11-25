namespace MandalorianBankomaten;

public interface IAccount // Interface for Account
{
    string AccountName { get; }
    decimal Balance { get; }
    string CurrencyCode { get; }

    void Deposit(decimal amount); // Method for deposit
    void Withdraw(decimal amount); // Method for withdrawal
}