using System.Globalization;

namespace MandalorianBankomaten
{
    public class User : IUser // User class implements IUser interface
    {
        private List<Account> _accounts;
        private List<Loan> _loans;
        public string Name { get; private set; }
        public string Password { get; private set; }
        public int UserId { get; private set; }
        public List<Account> Accounts => _accounts;
        public List<Loan> Loans => _loans;
        static int _userCounter = 0;

        public User(string name, string password)
        {
            _userCounter++;
            Name = name;
            Password = password;
            _accounts = new List<Account>();
            _loans = new List<Loan>();
            UserId = _userCounter;
        }

        public decimal MaxLoanAmount()
        {
            {
                // max loan amount is 5 times the total balance of all accounts
                return Accounts.Sum(account => account.Balance) * 5;
            }
        }

        // Show number of accounts
        public int NumberOfAccounts()
        {
            return _accounts.Count;
        }

        // See if user has accounts
        public bool HasAccounts()
        {
            if (_accounts.Count == 0)
            {
                Console.WriteLine($"{Name} har inga konton på den här banken.");
                return false;
            }
            return true;
        }

        // Method to show accounts
        public void ShowAccounts()
        {
            Console.WriteLine($"Konton för användare: {Name}");
            for (int i = 0; i < _accounts.Count; i++)
            {
                Console.WriteLine($"{_accounts[i].AccountID}. {_accounts[i].AccountName} - Saldo: {_accounts[i].Balance:C}");
            }
            if (Loans.Count > 0)
            {
                Console.WriteLine("\nLånekonton:");

                for (int i = 0; i < Loans.Count; i++)
                {
                    Console.WriteLine($"{Loans[i].LoanId}. Belopp: {Loans[i].Amount:C} - Ränta: {Loans[i].InterestRate}% - Saldo: {Loans[i].RemainingBalance:C}");
                }
            }
        }

        public void CreateAccount() // Tim 
        {
            string accountName;
            while (true)
            {
                Console.WriteLine("Ange namn på konto:");
                accountName = Console.ReadLine();

                // Check for invalid or empty account name
                if (string.IsNullOrWhiteSpace(accountName))
                {
                    Console.WriteLine("Input är ogiltigt. Vänligen ange ett giltigt namn för kontot.");
                }
                else
                {
                    break; // Exit the loop when a valid name is entered
                }
            }

            // Ask for the currency code
            string currencyCode;
            while (true)
            {
                Console.WriteLine("Ange konto-valuta (t.ex. SEK, USD, EUR):");
                currencyCode = Console.ReadLine().ToUpper(); // Convert to upper case to ensure consistency

                // You can extend this to validate known currencies, if needed.
                if (string.IsNullOrWhiteSpace(currencyCode))
                {
                    Console.WriteLine("Valutan kan inte vara tom. Vänligen ange en giltig valuta.");
                }
                else
                {
                    break;
                }
            }

            decimal depositAmount;
            while (true)
            {
                Console.WriteLine("Ange insättningsbelopp:");
                string input = Console.ReadLine();

                // Try to convert the input to a decimal, and handle invalid inputs.
                if (decimal.TryParse(input, out depositAmount) && depositAmount > 0)
                {
                    break; // Valid amount entered, exit the loop
                }
                else
                {
                    Console.WriteLine("Ogiltigt belopp. Vänligen ange ett positivt belopp för insättning.");
                }
            }

            // Create the new account
            Account newAccount = new Account(accountName, depositAmount, currencyCode);
            _accounts.Add(newAccount);

            // Inform the user that the account was created successfully
            Console.WriteLine($"Konto '{accountName}' har skapats med {depositAmount} {currencyCode}!");

        }
        public void AddAccount(Account account)
        {
            _accounts.Add(account);
        }

        public void RemoveAccount(Account account)
        {
            _accounts.Remove(account);
        }

        // Method to transfer money between two internal accounts
        public void TransferMoneyBetweenAccounts(Account fromAccount, Account toAccount, decimal amount)
        {
            if (fromAccount.Balance >= amount)
            {
                //Execute transfer
                fromAccount.Withdraw(amount);
                toAccount.Deposit(amount);
                Console.WriteLine($"Överföring från {fromAccount.AccountName} till {toAccount.AccountName} lyckades.");
            }
            else
            {
                Console.WriteLine("Otillräckligt saldo för överföring.");
            }
        }
        // Method to transfer money to an external account (i.e. other user)
        public void TransferMoneyToAccount(Account fromAccount, Account recipientAccount, decimal amount)
        {
            // Kontrollera om beloppet är positivt
            if (amount <= 0)
            {
                Console.WriteLine("Beloppet måste vara ett positivt tal.");
                return;
            }

            // Kontrollera om avsändarkontot har tillräckligt med saldo
            if (fromAccount.Balance < amount)
            {
                Console.WriteLine("Det finns inte tillräckligt med pengar på ditt konto för denna överföring.");
                return;
            }

            // Subtrahera beloppet från avsändarkontot
            fromAccount.Balance -= amount;

            // Lägg till beloppet till mottagarkontot
            recipientAccount.Balance += amount;
        }

        // Method to take a loan
        public void TakeLoan(decimal amount, decimal interestRate)
        {
            Console.WriteLine("Innan ditt lån kan genomföras behöver vi skapa upp ett unik lånekonto åt dig.");

            decimal totalLoanAmount = Loans.Sum(loan => loan.RemainingBalance);
            if (totalLoanAmount + amount > MaxLoanAmount())
            {
                Console.WriteLine($"Du kan max låna {MaxLoanAmount().ToString("C", CultureInfo.CurrentCulture)}.");
                return;
            }

            Loan newLoan = new Loan(amount, interestRate);
            Loans.Add(newLoan);

            Console.WriteLine($"Ett nytt lånekonto har skapats. Låne-ID: {newLoan.LoanId}");
            Console.WriteLine($"Lånebelopp: {newLoan.Amount.ToString("C", CultureInfo.CurrentCulture)}");
            Console.WriteLine($"Ränta: {newLoan.InterestRate}%");
            Console.WriteLine($"Månatlig ränta: {newLoan.MonthlyInterest().ToString("C", CultureInfo.CurrentCulture)}");
        }

        // Method to show loans
        public void ShowLoans()
        {
            if (Loans.Count == 0)
            {
                Console.WriteLine("Inga lån registrerade.");
                return;
            }

            foreach (var loan in Loans)
            {
                Console.WriteLine($"Lån: {loan.Amount} SEK, Ränta: {loan.InterestRate}%, Saldo: {loan.RemainingBalance} SEK");
            }
        }

    }

}