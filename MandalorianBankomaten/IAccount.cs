namespace MandalorianBankomaten;

public interface IAccount // Interface for Account
{
    string AccountName { get; }
    decimal Balance { get; }
    string CurrencyCode { get; }

}