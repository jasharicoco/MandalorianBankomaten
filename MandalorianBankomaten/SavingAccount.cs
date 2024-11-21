namespace MandalorianBankomaten
{
    public class SavingAccount : Account
    {
        public decimal InterestRate { get; set; }

        public SavingAccount(string accountName, decimal balance, string currency, decimal interestRate)
            : base(accountName, balance, currency)
        {
            if (interestRate < 0) throw new ArgumentException("Räntan kan inte vara negativ");
            InterestRate = interestRate;
        }         

        // Method to calculate interest
        public void ApplyInterest()
        {
            decimal totalInterest = Balance * InterestRate / 100;

            Deposit(totalInterest); 
            Console.WriteLine($"Räntesats: {InterestRate} procent");
            Console.WriteLine($"Nytt saldo inklusive ränta: {totalInterest}:C");
        }
    }
}
