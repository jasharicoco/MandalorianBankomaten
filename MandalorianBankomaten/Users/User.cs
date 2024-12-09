using MandalorianBankomaten.Accounts;
using MandalorianBankomaten.Loans;
using System.Globalization;
using System.Security.Principal;

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
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(49, 4);
            int yPosition = 4;
            Console.Write($"Konton för användare: {Name}");
            for (int i = 0; i < Accounts.Count; i++)
            {
                yPosition = yPosition + 1;
                Console.SetCursorPosition(49, yPosition);
                Console.Write($"{Accounts[i].AccountID}. {Accounts[i].AccountName} - Saldo: {CurrencyConverter.FormatAmount(Accounts[i].Balance, Accounts[i].CurrencyCode)}");
            }
            if (Loans.Count > 0)
            {
                yPosition = yPosition + Accounts.Count + 2;
                Console.SetCursorPosition(49, (yPosition));
                Console.Write("\nLånekonton:");
                
                for (int i = 0; i < Loans.Count; i++)
                {
                    yPosition = yPosition + 1;
                    Console.SetCursorPosition(49, yPosition);
                    Console.Write(Loans[i].ToString());
                }
            }
        }
        public void CreateAccount()
        {
            // Menu to choose type of account
            Console.WriteLine("Välj typ av konto du vill skapa:");
            Console.WriteLine("[1] Vanligt konto");
            Console.WriteLine("[2] Sparkonto");

            int accountType;
            while (true)
            {
                Console.Write("Ditt val: ");
                string userChoice = Console.ReadLine();

                if (int.TryParse(userChoice, out accountType) && accountType == 1 || accountType == 2)
                {
                    break; // Exit loop when valid choice is made
                }
                else
                {
                    Console.WriteLine("Ogiltigt val. Välj [1] eller [2]"); // Invalid choice
                }
            }
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

            string currencyCode;
            decimal depositAmount;

            // Valid currency codes in an array for easy checking
            string[] validCurrencies = { "SEK", "USD", "EUR", "GBP", "DKK", "JPY" };

            do
            {
                    Console.WriteLine("Ange konto-valuta (SEK, USD, EUR, GBP, DKK, JPY)");
                    currencyCode = Console.ReadLine().ToUpper(); // Convert to upper case to ensure consistency

            }
            while (!validCurrencies.Contains(currencyCode)); // Check if input is in the list of valid currencies

            while (true)
            {

                Console.WriteLine("Ange insättningsbelopp:");
                string input = Console.ReadLine();

                // Try to convert the input to a decimal, and handle invalid inputs.
                if (decimal.TryParse(input, out depositAmount) && depositAmount > 0)
                {
                    CurrencyConverter.FormatAmount(depositAmount, currencyCode);
                    break; // Valid amount entered, exit the loop
                }
                else
                {
                    Console.WriteLine("Ogiltigt belopp. Vänligen ange ett positivt belopp för insättning.");
                }
            }

            if (accountType == 1)
            {
                // Create the new account
                Account newAccount = new Account(accountName, currencyCode, depositAmount);
                Accounts.Add(newAccount);

                // Inform the user that the account was created successfully
                Console.WriteLine($"Konto '{accountName}' har skapats med {CurrencyConverter.FormatAmount(depositAmount, currencyCode)}!");
            }
            else if (accountType == 2)
            {
                // Hardcoded interestrate
                decimal interestRate = 0.05m; // Interest rate at 5%
                SavingAccount newSavingAccount = new SavingAccount(accountName, currencyCode, depositAmount, interestRate);
                Accounts.Add(newSavingAccount);

                // Inform the user that saving account was created successfully
                Console.WriteLine($"Sparkonto '{accountName}' har skapats.");
                newSavingAccount.ApplyInterest();
            }

        }
        public void RemoveAccount()
        {
            ShowAccounts();
            Console.WriteLine();
            Console.WriteLine("Skriv in kontonummret på kontot du vill ta bort:");

            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out int accountID))
                {
                    Console.WriteLine("Fel inmatning, försök igen!");
                }
                else
                {
                    foreach (var account in Accounts)
                    {
                        if (account.AccountID == accountID)
                        {
                            // Gets the specified account from users accounts list
                            Account accountToRemove = Accounts.FirstOrDefault(account => account.AccountID == accountID);


                            Console.WriteLine();
                            Console.WriteLine("Är du säker på att du vill ta bort följande konto (j/n):");
                            Console.WriteLine($"{accountToRemove.AccountName} - {accountToRemove.Balance:C}");
                            while (true)
                            {
                                string choiceConfirm = Console.ReadLine().ToLower();
                                if (choiceConfirm == "j")
                                {
                                    break;
                                }
                                else if (choiceConfirm == "n")
                                {
                                    Console.WriteLine("Borttagandet av kontot avbröts.");
                                    return;
                                }
                                else
                                {
                                    Console.WriteLine("Fel inmatning, försök igen!");
                                }
                            }

                            Accounts.Remove(accountToRemove);
                            Console.WriteLine("Kontot har tagits bort!");
                            return;
                        }
                        else
                        {
                            Console.WriteLine("Fel kontonummer, försök igen!");
                        }
                    }
                }
            }
        }
        public void AddAccount(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            Accounts.Add(account);
        }
        //between users
        //this
        public void TransferMoneyBetweenAccounts(Account fromAccount, Account toAccount, decimal amount)
        {

            if (fromAccount.Balance >= amount)
            {
                //Execute transfer
                Console.WriteLine($"Överföring från {fromAccount.AccountName} till {toAccount.AccountName} lyckades.");

                CurrencyConverter.Converter(fromAccount.CurrencyCode, toAccount.CurrencyCode, amount);
                fromAccount.Withdraw(amount);
                toAccount.Deposit(amount);
            }
            else
            {
                Console.WriteLine("Otillräckligt saldo för överföring.");
            }

            decimal converterAmount = CurrencyConverter.Converter(fromAccount.CurrencyCode, toAccount.CurrencyCode, amount);

            fromAccount.Withdraw(converterAmount);
            toAccount.Deposit(converterAmount);

        }

        //this
        //between own accounts 
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

            if (!CurrencyConverter.ValidateCurrency(fromAccount.CurrencyCode, recipientAccount.CurrencyCode))
            {
                return;
            }

            decimal converterAmount = CurrencyConverter.Converter(fromAccount.CurrencyCode, recipientAccount.CurrencyCode, amount);

            // Subtrahera beloppet från avsändarkontot
            fromAccount.Withdraw(converterAmount);
            // Lägg till beloppet till mottagarkontot
            recipientAccount.Deposit(converterAmount);

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

        public void MakeAmortization(Loan loan, Account account, decimal amount)
        {
            account.Withdraw(amount); // withdraw from account
            loan.MakePayment(amount); // make payment on loan
        }
        public override string ToString()
        {
            return $"Name: {Name}, User ID: {UserId}";
        }

    }
}
