namespace MandalorianBankomaten;

public class Account
{
    public string AccountName { get; set; }
    public decimal AccountMoney { get; set; }
    public string Currency { get; set; }

    public Account(string accountName, decimal accountMoney, string currency)
    {
        AccountName = accountName;
        AccountMoney = accountMoney;
        Currency = currency;
    }
}