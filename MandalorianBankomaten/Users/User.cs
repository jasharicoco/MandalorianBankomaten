using MandalorianBankomaten.Accounts;
using MandalorianBankomaten.Loans;
using MandalorianBankomaten.Menu;
using System;
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
                MenuUtility.CustomWriteLine(49, $"{Name} har inga konton på den här banken.");
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
            Console.SetCursorPosition(49, 4);
            int yPosition = 4;
            MenuUtility.CustomWriteLine(49, $"Konton för användare: {Name}");
            for (int i = 0; i < Accounts.Count; i++)
            {
                MenuUtility.CustomWriteLine(49, $" {Accounts[i].AccountID}. {Accounts[i].AccountName} - Saldo: {CurrencyConverter.FormatAmount(Accounts[i].Balance, Accounts[i].CurrencyCode)}");
            }
            if (Loans.Count > 0)
            {

                Console.WriteLine();
                MenuUtility.CustomWriteLine(49, "Lånekonton:");
                
                for (int i = 0; i < Loans.Count; i++)
                {
                    MenuUtility.CustomWriteLine(49, Loans[i].ToString());
                }
            }
        }
        public void CreateAccount()
        {

            // Menu to choose type of account
            Console.SetCursorPosition(49, 4);
            MenuUtility.CustomWriteLine(49, "Välj typ av konto du vill skapa:");
            MenuUtility.CustomWriteLine(49, "[1] Vanligt konto");
            MenuUtility.CustomWriteLine(49, "[2] Sparkonto med 5% ränta");

            int accountType;
            while (true)
            {
                MenuUtility.CustomWriteLine(49, "Ditt val: ");
                string userChoice = MenuUtility.CustomReadLine("Ditt val: ".Length);

                if (int.TryParse(userChoice, out accountType) && accountType == 1 || accountType == 2)
                {
                    break; // Exit loop when valid choice is made
                }
                else
                {
                    MenuUtility.CustomWriteLine(49, "Ogiltigt val. Välj [1] eller [2]"); // Invalid choice
                }
            }
            string accountName;
            while (true)
            {
                MenuUtility.CustomWriteLine(49, "Ange namn på konto:");
                accountName = MenuUtility.CustomReadLine("Ange namn på konto:".Length);

                // Check for invalid or empty account name
                if (string.IsNullOrWhiteSpace(accountName))
                {
                    MenuUtility.CustomWriteLine(49, "Input är ogiltigt. Vänligen ange ett giltigt namn för kontot.");
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
                MenuUtility.CustomWriteLine(49, "Ange konto-valuta (SEK, USD, EUR, GBP, DKK, JPY):");
                currencyCode = MenuUtility.CustomReadLine("Ange konto-valuta (SEK, USD, EUR, GBP, DKK, JPY):".Length).ToUpper(); // Convert to upper case to ensure consistency
            }
            while (!validCurrencies.Contains(currencyCode)); // Check if input is in the list of valid currencies

            while (true)
            {

                MenuUtility.CustomWriteLine(49, "Ange insättningsbelopp:");
                string input = MenuUtility.CustomReadLine("Ange insättningsbelopp:".Length);

                // Try to convert the input to a decimal, and handle invalid inputs.
                if (decimal.TryParse(input, out depositAmount) && depositAmount > 0)
                {
                    CurrencyConverter.FormatAmount(depositAmount, currencyCode);
                    break; // Valid amount entered, exit the loop
                }
                else
                {
                    MenuUtility.CustomWriteLine(49, "Ogiltigt belopp. Vänligen ange ett positivt belopp för insättning.");
                }
            }

            if (accountType == 1)
            {
                // Create the new account
                Account newAccount = new Account(accountName, currencyCode, depositAmount);
                Accounts.Add(newAccount);
                Console.WriteLine();
                // Inform the user that the account was created successfully
                MenuUtility.CustomWriteLine(49, $"Konto '{accountName}' har skapats med {CurrencyConverter.FormatAmount(depositAmount, currencyCode)}!");
            }
            else if (accountType == 2)
            {
                // Hardcoded interestrate
                decimal interestRate = 0.05m; // Interest rate at 5%
                SavingAccount newSavingAccount = new SavingAccount(accountName, currencyCode, depositAmount, interestRate);
                Accounts.Add(newSavingAccount);

                // Inform the user that saving account was created successfully
                MenuUtility.CustomWriteLine(49, $"Sparkonto '{accountName}' har skapats.");
                newSavingAccount.ApplyInterest();
            }

        }
        public void RemoveAccount()
        {
            Console.SetCursorPosition(49, 4);
            ShowAccounts();
            MenuUtility.CustomWriteLine(49, "");
            MenuUtility.CustomWriteLine(49, "Skriv in kontonummret på kontot du vill ta bort:");

            while (true)
            {
                string input = MenuUtility.CustomReadLine("Skriv in kontonummret på kontot du vill ta bort:".Length);
                if (input == "exit")
                {
                    return;
                }
                if (!int.TryParse(input, out int accountID))
                {
                    MenuUtility.CustomWriteLine(49, "Fel inmatning. Skriv 'exit' för att avbryta.");
                }
                else
                {
                    bool accountFound = false; // Flagga för att spåra om kontot hittades

                    foreach (var account in Accounts)
                    {
                        if (account.AccountID == accountID)
                        {
                            accountFound = true; // Konto hittades

                            // Gets the specified account from users accounts list
                            Account accountToRemove = Accounts.FirstOrDefault(account => account.AccountID == accountID);

                            Console.WriteLine();
                            MenuUtility.CustomWriteLine(49, $"{accountToRemove.AccountName} - {accountToRemove.Balance:C}");
                            MenuUtility.CustomWriteLine(49, "Är du säker på att du vill ta bort följande konto (j/n):");
                            while (true)
                            {
                                string choiceConfirm = MenuUtility.CustomReadLine("Är du säker på att du vill ta bort följande konto (j/n):".Length).ToLower();
                                if (choiceConfirm == "j")
                                {
                                    break;
                                }
                                else if (choiceConfirm == "n")
                                {
                                    MenuUtility.CustomWriteLine(49, "Borttagandet av kontot avbröts.");
                                    return;
                                }
                                else
                                {
                                    MenuUtility.CustomWriteLine(49, "Fel inmatning, försök igen!");
                                }
                            }

                            Accounts.Remove(accountToRemove);
                            Console.WriteLine();
                            MenuUtility.CustomWriteLine(49, "Kontot har tagits bort!");
                            return;
                        }
                    }

                    // Om inget konto hittades
                    if (!accountFound)
                    {
                        MenuUtility.CustomWriteLine(49, "Fel kontonummer, försök igen!");
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
        public void TransferMoneyBetweenAccounts(Account fromAccount, Account toAccount, decimal amount, decimal amountConverted)
        {
            if (fromAccount.Balance >= amount)
            {
                Console.WriteLine();
                //Execute transfer
                MenuUtility.CustomWriteLine(49, $"Överföring från {fromAccount.AccountName} till {toAccount.AccountName} lyckades.");

                fromAccount.Withdraw(amount);
                toAccount.Deposit(amountConverted);
            }
            else
            {
                MenuUtility.CustomWriteLine(49, "Kontosaldo otillräckligt för överföring.");
            }
        }

        //this
        //between own accounts 
        public void TransferMoneyToAccount(Account fromAccount, Account recipientAccount, decimal amount, decimal amountConverted)
        {
            // Subtrahera beloppet från avsändarkontot
            if(!CurrencyConverter.ValidateCurrency(fromAccount.CurrencyCode, recipientAccount.CurrencyCode))
            {
                return;
            }

            fromAccount.Withdraw(amount);
            // Lägg till beloppet till mottagarkontot
            recipientAccount.DepositSecret(amountConverted);
        }

        public void TakeLoan(decimal amount, Loan.LoanCategory loanCategory)
        {
            //MenuUtility.CustomWriteLine(49, "Innan ditt lån kan genomföras behöver vi skapa upp ett unik lånekonto åt dig.");

            Loan newLoan = new Loan(amount, loanCategory);
            Loans.Add(newLoan);

            MenuUtility.CustomWriteLine(49, $"Ett nytt {loanCategory} har skapats. Låne-ID: {newLoan.LoanId}");
            MenuUtility.CustomWriteLine(49, $"Lånebelopp: {newLoan.Amount.ToString("C", CultureInfo.CurrentCulture)}");
            MenuUtility.CustomWriteLine(49, $"Ränta: {newLoan.InterestRate}%");
            MenuUtility.CustomWriteLine(49, $"Månatlig ränta: {newLoan.MonthlyInterest().ToString("C", CultureInfo.CurrentCulture)}");
        }
        public void ShowLoans()
        {
            if (Loans.Count == 0)
            {
                MenuUtility.CustomWriteLine(49, "Inga lån registrerade.");
                return;
            }

            foreach (var loan in Loans)
            {
                MenuUtility.CustomWriteLine(49, loan.ToString());
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
