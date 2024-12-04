namespace MandalorianBankomaten.Loans
{
    internal class Loan
    {
        // Private fields
        private decimal _amount;
        private decimal _interestRate;
        private decimal _remainingBalance;
        private decimal _loanId;

        // Static counter for generating unique loan IDs
        private static int _loanCounter = 8540;

        // Public properties
        public LoanCategory Category { get; private set; }
        public decimal LoanId
        {
            get => _loanId;
            private set
            {
                _loanId = value;
            }
        }
        public decimal Amount
        {
            get => _amount;
            private set
            {
                //if (value <= 0)
                //throw new ArgumentException("Loan amount must be greater than zero.");
                _amount = value;
            }
        }
        public decimal InterestRate
        {
            get => _interestRate;
            private set
            {
                //if (value < 0 || value > 100)
                //throw new ArgumentException("Interest rate must be between 0 and 100.");
                _interestRate = value;
            }
        }
        public decimal RemainingBalance
        {
            get => _remainingBalance;
            private set
            {
                //if (value < 0)
                //throw new ArgumentException("Remaining balance cannot be negative.");
                _remainingBalance = value;
            }
        }

        // Enumeration
        public enum LoanCategory
        {
            Bolån,
            Billån,
            Privatlån
        }

        // Constructor
        public Loan(decimal amount, LoanCategory loanCategory)
        {
            LoanId = _loanCounter;
            Amount = amount;
            InterestRate = GetInterestRateByType(loanCategory);
            Category = loanCategory;
            RemainingBalance = amount;
            _loanCounter++;
        }

        // Methods
        private decimal GetInterestRateByType(LoanCategory loanCategory)
        {
            return loanCategory switch
            {
                LoanCategory.Bolån => 4.0m, // Bolån
                LoanCategory.Billån => 8.0m,  // Billån
                LoanCategory.Privatlån => 10.0m, // Privatlån
                _ => throw new ArgumentException("Invalid loan type.")
            };
        }
        public decimal MonthlyInterest()
        {
            return RemainingBalance * (InterestRate / 100) / 12;
        }
        public void MakePayment(decimal paymentAmount) { }
        public override string ToString()
        {
            return $"Lån ID: {LoanId}, Typ av lån: {Category}, Summa: {Amount:C}, Ränta: {InterestRate}%, Låneutrymme kvar: {RemainingBalance:C}";
        }

    }
}
