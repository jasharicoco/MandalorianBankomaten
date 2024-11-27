namespace MandalorianBankomaten;

public class Loan
{
    private static int _loanCounter = 0;
    public int LoanId { get; private set; }
    public decimal Amount { get; private set; } 
    public decimal InterestRate { get; private set; }
    public decimal RemainingBalance { get; private set; }
    
    public Loan(decimal amount, decimal interestRate)
    {
        if (amount <= 0) throw new ArgumentException("Lånebeloppet måste vara större än noll.", nameof(amount)); 
        if (interestRate <= 0) throw new ArgumentException("Räntan måste vara större än noll.", nameof(interestRate));
        
        LoanId = ++_loanCounter;
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
    public void MakePayment(decimal paymentAmount)
    {
        if (paymentAmount <= 0) throw new ArgumentException("Betalningsbeloppet måste vara större än noll.");
        RemainingBalance -= paymentAmount;
        if (RemainingBalance < 0) 
            RemainingBalance = 0;
    }
}