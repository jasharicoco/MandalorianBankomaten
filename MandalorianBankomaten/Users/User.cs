using MandalorianBankomaten.Accounts;
using MandalorianBankomaten.Loans;
using System.Globalization;

namespace MandalorianBankomaten.Users
{
    internal class User
    {
        // Private fields
        private List<Account> _accounts;
        private List<Loan> _loans;
        private string _name;
        private string _password;
        private int _userId;

        // Static counter for generating unique user IDs
        private static int _userCounter = 0;

        // Public properties
        public List<Account> Accounts => _accounts ??= new List<Account>();
        public List<Loan> Loans => _loans ??= new List<Loan>();
        public string Name
        {
            get => _name;
            private set
            {
                //if (string.IsNullOrWhiteSpace(value))
                //throw new ArgumentException("Name cannot be null or empty.");
                _name = value;
            }
        }
        public string Password
        {
            get => _password;
            private set
            {
                //if (string.IsNullOrWhiteSpace(value))
                //throw new ArgumentException("Password cannot be null or empty.");
                _password = value;
            }
        }
        public int UserId
        {
            get => _userId;
            private set => _userId = value;
        }

        // Constructor
        public User(string name, string password)
        {
            UserId = _userCounter;
            Name = name;
            Password = password;
            _userCounter++;
        }

        // Methods
        public decimal MaxLoanAmount()
        {
            // Max loan amount is 5 times the total balance of all accounts
            return Accounts.Sum(account => account.Balance) * 5;
        }
        public bool HasAccounts()
        {
            if (Accounts.Count == 0)
            {
                Console.WriteLine($"{Name} har inga konton på den här banken.");
                return false;
            }
            return true;
        }
        public int NumberOfAccounts()
        {
            return Accounts.Count;
        }
        public void ShowAccounts()
        {
            Console.WriteLine($"Konton för användare: {Name}");
            for (int i = 0; i < Accounts.Count; i++)
            {
                Console.WriteLine($"{Accounts[i].AccountID}. {Accounts[i].AccountName} - Saldo: {Accounts[i].Balance:C}");
            }
            if (Loans.Count > 0)
            {
                Console.WriteLine("\nLånekonton:");

                for (int i = 0; i < Loans.Count; i++)
                {
                    Console.WriteLine(Loans[i].ToString());
                }
            }
        }
        public void CreateAccount()
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
                else if (currencyCode == "SEK" || currencyCode == "USD" || currencyCode == "EUR")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Ange giltlig valuta.");
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
            Account newAccount = new Account(accountName, currencyCode, depositAmount);
            Accounts.Add(newAccount);

            // Inform the user that the account was created successfully
            Console.WriteLine($"Konto '{accountName}' har skapats med {depositAmount} {currencyCode}!");

        }
        public void AddAccount(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            Accounts.Add(account);
        }
        public void RemoveAccount(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            Accounts.Remove(account);
        }
        public void TransferMoneyBetweenAccounts(Account fromAccount, Account toAccount, decimal amount)
        {
            if (fromAccount.Balance >= amount)
            {
                //Execute transfer
                Console.WriteLine($"Överföring från {fromAccount.AccountName} till {toAccount.AccountName} lyckades.");
                fromAccount.Withdraw(amount);
                toAccount.Deposit(amount);
            }
            else
            {
                Console.WriteLine("Otillräckligt saldo för överföring.");
            }
        }
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
        public void TakeLoan(decimal amount, Loan.LoanCategory loanCategory)
        {
            Console.WriteLine("Innan ditt lån kan genomföras behöver vi skapa upp ett unik lånekonto åt dig.");
            
            Loan newLoan = new Loan(amount, loanCategory);
            Loans.Add(newLoan);

            Console.WriteLine($"Ett nytt {loanCategory} har skapats. Låne-ID: {newLoan.LoanId}");
            Console.WriteLine($"Lånebelopp: {newLoan.Amount.ToString("C", CultureInfo.CurrentCulture)}");
            Console.WriteLine($"Ränta: {newLoan.InterestRate}%");
            Console.WriteLine($"Månatlig ränta: {newLoan.MonthlyInterest().ToString("C", CultureInfo.CurrentCulture)}");
        }
        public void ShowLoans()
        {
            if (Loans.Count == 0)
            {
                Console.WriteLine("Inga lån registrerade.");
                return;
            }

            foreach (var loan in Loans)
            {
                Console.WriteLine(loan.ToString());
            }
        }
        public override string ToString()
        {
            return $"Name: {Name}, User ID: {UserId}";
        }

    }
}
