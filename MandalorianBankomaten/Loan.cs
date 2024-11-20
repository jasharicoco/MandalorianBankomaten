namespace MandalorianBankomaten;

public class Loan
{
    public decimal Amount { get; private set; } 
    public decimal InterestRate { get; private set; }
    public decimal RemainingBalance { get; private set; }
    
    public Loan(decimal amount, decimal interestRate)
    {
        if (amount <= 0) throw new ArgumentException("Lånebeloppet måste vara större än noll.", nameof(amount));
        
        Amount = amount;
        InterestRate = interestRate;
        RemainingBalance = amount;
    }
    
    // Method to calculate the monthly interest
    public decimal MonthlyInterest()
    {
        return RemainingBalance * (InterestRate / 100 / 12);
    }
    // Method to calculate the total amount to pay back
    public void MakePay(decimal payment)
    {
        if (payment <= 0) throw new ArgumentException("Betalningsbeloppet måste vara större än noll.");
        RemainingBalance -= payment;
        if (RemainingBalance < 0) 
            RemainingBalance = 0;
    }
}