namespace MandalorianBankomaten;
public class Loan
{
    // Private fields
    private decimal _amount;
    private decimal _interestRate;
    private decimal _remainingBalance;

    // Static counter for generating unique loan IDs
    private static int _loanCounter = 8540;

    // Public properties
    public int LoanId { get; private set; }

    public decimal Amount
    {
        get => _amount;
        private set
        {
            if (value <= 0)
                throw new ArgumentException("Loan amount must be greater than zero.");
            _amount = value;
        }
    }

    public decimal InterestRate
    {
        get => _interestRate;
        private set
        {
            if (value < 0 || value > 100)
                throw new ArgumentException("Interest rate must be between 0 and 100.");
            _interestRate = value;
        }
    }

    public decimal RemainingBalance
    {
        get => _remainingBalance;
        private set
        {
            if (value < 0)
                throw new ArgumentException("Remaining balance cannot be negative.");
            _remainingBalance = value;
        }
    }

    // Constructor
    public Loan(decimal amount, decimal interestRate)
    {
        LoanId = _loanCounter;
        Amount = amount;
        InterestRate = interestRate;
        RemainingBalance = amount;
        _loanCounter++;
    }

    // Methods
    public decimal MonthlyInterest()
    {
        return RemainingBalance * (InterestRate / 100) / 12;}
    public void MakePayment(decimal paymentAmount) { }
    public override string ToString()
    {
        return $"Amount: {Amount}, Interest Rate: {InterestRate}, Remaining Balance: {RemainingBalance}, Loan ID: {LoanId}";
    }
}