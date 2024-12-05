using MandalorianBankomaten.Accounts;
using MandalorianBankomaten.Helpers;
using MandalorianBankomaten.Loans;
using MandalorianBankomaten.Menu;
using MandalorianBankomaten.Transactions;
using MandalorianBankomaten.Users;
using System.Globalization;
using System.Text;

namespace MandalorianBankomaten
{
    internal class Bank
    {
        // Private fields
        private List<User> _users;
        private List<Admin> _admins;
        private User _currentUser;
        private Admin _currentAdmin;
        private TransactionLog _transactionLog;

        // Public properties
        public List<User> Users
        {
            get => _users;
            private set => _users = value;
        }

        public List<Admin> Admins
        {
            get => _admins;
            private set => _admins = value;
        }

        public User CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        public Admin CurrentAdmin
        {
            get => _currentAdmin;
            set => _currentAdmin = value;
        }

        public TransactionLog TransactionLog
        {
            get => _transactionLog;
            private set => _transactionLog = value;
        }

        // Constructor
        public Bank()
        {
            Users = Seeder.SeedUsers(); // Initialize users and admins from AccountSeeder
            Admins = Seeder.SeedAdmins(); // Initialize users and admins from AccountSeeder
            Seeder.AddSeedAccountsToUsers(Users); // Add standard accounts to each user
        }

        // Methods
        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8; // Gör ovanliga symboler synliga i programmet
            MenuUtility.ASCIIArt();
            bool programRunning = true;
            string? choice;

            DisplayMessage("\ud83c\udf1f Välkommen till Mandalorian Bankomaten \ud83c\udf1f\n");

            bool loginSuccesfull = LogIn();
            if (loginSuccesfull) // if login is successful
            {
                while (programRunning)
                {
                    Console.Clear();
                    ConsoleKey key = ConsoleKey.A;
                    int choiceIndex = 0;
                    //menyvalen indexas. En for-loop går igenom menyn och skriver ut den rad som indexet står på med en färg,
                    //och skriver ut resten utan färg.
                    string[] menu = {"1. Visa konton\n" ,
                        "2. Lägg till konto\n" ,
                        "3. Ta bort konto\n" ,
                        "4. För över pengar mellan konton\n" ,
                        "5. För över pengar till en annan användare\n" ,
                        "6. Ta lån\n" ,
                        "7. Amortera lån\n" ,
                        "8. Logga ut\n" };

                    if (CurrentAdmin != null)
                    {
                        string[] adminMenu = {"1. Skapa användare\n" ,
                                               "2. Radera användare\n" ,
                                               "3. Logga ut\n" };

                        // Admin Menu
                        while (programRunning)
                        {
                            //(choiceIndex, key) are the values that return from the method having been changed from the input
                            (choiceIndex, key) = MenuUtility.ShowMenu(adminMenu, choiceIndex, key);

                            if (key == ConsoleKey.Enter)
                            {
                                //adminChoiceIndex is 0 when the first option is highlighted, hence the + 1. 
                                switch (choiceIndex + 1)
                                {
                                    case 1:
                                        Users = CurrentAdmin.CreateUser(Users);
                                        break;
                                    case 2:
                                        Users = CurrentAdmin.DeleteUser(Users);
                                        break;
                                    case 3:
                                        CurrentAdmin = null;
                                        loginSuccesfull = LogIn();
                                        break;
                                    default:
                                        Console.WriteLine("Ogiltligt menyval. Försök igen!");
                                        break;
                                }
                            }
                        }
                    }
                    else
                    {
                        // User Menu
                        while (programRunning)
                        {
                            (choiceIndex, key) = MenuUtility.ShowMenu(menu, choiceIndex, key);
                            
                            if (key == ConsoleKey.Enter)
                            {
                                Console.Clear();
                                switch (choiceIndex + 1)
                                {
                                    case 1:
                                        CurrentUser.ShowAccounts();
                                        Return();
                                        break;
                                    case 2:
                                        CurrentUser.CreateAccount();
                                        Return();
                                        break;
                                    case 3:
                                        //currentUser.RemoveAccount();
                                        Return();
                                        break;
                                    case 4:
                                        TransferBetweenAccounts();
                                        Return();
                                        break;
                                    case 5:
                                        TransferToAnotherUser();
                                        Return();
                                        break;
                                    case 6:
                                        TakeLoan();
                                        Return();
                                        break;
                                    case 7:
                                        AmortizeLoan(CurrentUser);
                                        Return();
                                        break;
                                    case 8:
                                        CurrentUser = null;
                                        loginSuccesfull = LogIn();
                                        break;
                                    default:
                                        Console.WriteLine("Ogiltligt menyval. Försök igen!");
                                        Return();
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }
        public bool LogIn()
        {
            int attempts = 0;
            const int maxAttempts = 3; // Constant for max attempts 
            do
            {
                attempts++;
                DisplayMessage("Vänligen skriv in ditt \ud83d\udc64 användernamn: ");
                string username = Console.ReadLine().ToLower();
                DisplayMessage("Vänligen skriv in ditt \ud83d\udd12 lösenord: ");                string userpswd = Helper.ReadPassword();

                // First we check if it is an admin logging in
                foreach (var admin in Admins)
                {
                    if (username == admin.Name && userpswd == admin.Password)
                    {
                        MenuUtility.ShowSuccessMessageAdmin(username);
                        CurrentAdmin = admin;
                        return true;
                    }
                }

                // No admin match means we check the regular user-list for a match
                foreach (var user in Users)
                {
                    if (username == user.Name && userpswd == user.Password)
                    {
                        MenuUtility.ShowSuccessMessage(username);
                        CurrentUser = user;
                        return true;
                    }
                }
                MenuUtility.ShowFailedLoginMessage(maxAttempts - attempts);
            } while (attempts < 3);

            MenuUtility.ShowBlockedMessage();
            return false;
        }
        public void TransferToAnotherUser()
        {
            // Visa användarens konton för att välja avsändarkonto
            CurrentUser.ShowAccounts();

            DisplayMessage("Ange numret för kontot att överföra från: ");
            if (!int.TryParse(Console.ReadLine(), out int fromId) || fromId < 4850)
            {
                DisplayMessage("Ogiltigt val.", true);
                return;
            }
            if (fromId >= 8540)
            {
                DisplayMessage("Du kan ej överföra från ett lånekonto.", true);
                return;
            }
            // Hitta avsändarkontot med matchande AccountID
            var fromAccount = CurrentUser.Accounts.FirstOrDefault(account => account.AccountID == fromId);
            if (fromAccount == null)
            {
                DisplayMessage("Inget konto hittades med det numret.", true);
                return;
            }

            var allAccounts = new List<Account>();
            foreach (var user in Users)
            {
                allAccounts.AddRange(user.Accounts);
            }

            // Ange mottagarens kontonummer
            DisplayMessage("Ange numret för mottagarens konto: ");
            if (!int.TryParse(Console.ReadLine(), out int toId) || toId < 4850)
            {
                DisplayMessage("Ogiltigt val.", true);
                return;
            }

            // Hitta mottagarens konto med matchande AccountID
            var recipientAccount = allAccounts.FirstOrDefault(account => account.AccountID == toId);
            if (recipientAccount == null)
            {
                DisplayMessage("Inget konto hittades med det numret.", true);
                return;
            }

            // Hitta mottagarens användare genom att kolla vilket konto som tillhör vilken användare
            var recipient = Users.FirstOrDefault(user => user.Accounts.Contains(recipientAccount));
            if (recipient == null)
            {
                DisplayMessage("Mottagaren finns inte.", true);
                return;
            }

            // Ange belopp
            DisplayMessage("Ange belopp att överföra: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0 || amount > fromAccount.Balance)
            {
                DisplayMessage("Ogiltigt belopp. Beloppet måste vara positivt och mindre än eller lika med ditt saldo.", true);
                return;
            }

            // Utför överföringen
            CurrentUser.TransferMoneyToAccount(fromAccount, recipientAccount, amount);

            // Log the transaction----------
            string transactionInfo = $"Överföring: {amount} från konto {fromAccount} till konto {recipientAccount}";
            //TransactionLog.LogTransaction(transactionInfo);

            // Bekräftelse av överföringen
            DisplayMessage($"Du har skickat {amount:C} från konto {fromAccount.AccountID}: {fromAccount.AccountName} till konto {recipientAccount.AccountID}.");
            DisplayMessage($"Ditt nya saldo är för konto {fromAccount.AccountID}: {fromAccount.AccountName} är: {fromAccount.Balance:C}");
        }
        public void TransferBetweenAccounts()
        {
            // Visa användarens konton
            CurrentUser.ShowAccounts();

            // Välj avsändarkonto
            DisplayMessage("\nAnge numret för kontot att överföra från: ");
            if (!int.TryParse(Console.ReadLine(), out int fromId) || fromId < 4850)
            {
                DisplayMessage("Ogiltigt val.", true);
                return;
            }

            if (fromId >= 8540)
            {
                DisplayMessage("Du kan ej överföra från ett lånekonto.", true);
                return;
            }

            // Find the account with the matching accountID
            var fromAccount = CurrentUser.Accounts.FirstOrDefault(account => account.AccountID == fromId);

            if (fromAccount == null)
            {
                DisplayMessage("Inget konto hittades med det numret.", true);
                return;
            }

            // Välj mottagarkonto
            DisplayMessage("Ange numret för kontot att överföra till: ");
            if (!int.TryParse(Console.ReadLine(), out int toId) || toId < 4850 || toId == fromId)
            {
                DisplayMessage("Ogiltigt val.", true);
                return;
            }
            if (toId >= 8540)
            {
                DisplayMessage("Du kan ej överföra till ett lånekonto.", true);
                return;
            }
            var toAccount = CurrentUser.Accounts.FirstOrDefault(account => account.AccountID == toId);

            if (toAccount == null)
            {
                DisplayMessage("Inget konto hittades med det numret.", true);
                return;
            }
            // Ange belopp
            DisplayMessage("Ange belopp att överföra: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                DisplayMessage("Beloppet måste vara ett positivt tal.", true);
                return;
            }

            // Utför överföringen
            CurrentUser.TransferMoneyBetweenAccounts(fromAccount, toAccount, amount);

            // Log the transaction----------
            string transferInfo = $"Överföring: {amount} från konto {fromAccount} till konto {toAccount}";
            //TransactionLog.LogTransaction(transferInfo);
        }
        public void TakeLoan() // programmed by Alex & Tim
        {
            if (CurrentUser == null)
            {
                DisplayMessage("Du måste vara inloggad för att ta ett lån.", true);
                return;
            }
            DisplayMessage($"Hej {CurrentUser.Name}! Välkommen till bankens låneavdelning. Du kan låna upp till 5 gånger ditt totala saldo.");
            decimal maxLoanAmount = CurrentUser.Accounts.Sum(account => account.Balance) * 5;
            decimal currentLoanAmount = CurrentUser.Loans.Sum(loan => loan.RemainingBalance);
            decimal availableLoanAmount = maxLoanAmount - currentLoanAmount;

            DisplayMessage($"Ditt nuvarande låneutrymme: {availableLoanAmount.ToString("C", CultureInfo.CurrentCulture)}");

            decimal amount;
            Loan.LoanCategory loanCategory;
            // input for loan type
            DisplayMessage("Välj typ av lån:");
            DisplayMessage("1. 🏠 Bolån 4% ränta");
            DisplayMessage("2. 🚗 Billån 8% ränta");
            DisplayMessage("3. 💳 Privatlån 10% ränta");
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 3)
                {
                    break;
                }
                DisplayMessage("Ogiltigt val. Försök igen.", true);
            }

            switch (choice)
            {
                case 1:
                    loanCategory = Loan.LoanCategory.Bolån;
                    break;
                case 2:
                    loanCategory = Loan.LoanCategory.Billån;
                    break;
                case 3:
                    loanCategory = Loan.LoanCategory.Privatlån;
                    break;
                default: // Should never happen
                    return;
            }

            // input for loan amount
            do
            {
                DisplayMessage("Ange lånebelopp: ");
                if (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                {
                    DisplayMessage("Ogiltigt belopp. Försök igen.", true);
                    continue;
                }
                if (amount + currentLoanAmount > maxLoanAmount)
                {
                    DisplayMessage("Beloppet överstiger ditt tillgängliga låneutrymme. Försök igen.", true);
                }
                else if (amount > availableLoanAmount)
                {
                    DisplayMessage("Beloppet överstiger det lånebelopp du kan ta för den valda lånetypen. Försök igen.", true);
                }
            } while (amount <= 0 || amount + currentLoanAmount > maxLoanAmount || amount > availableLoanAmount);

            CurrentUser.TakeLoan(amount, loanCategory); // Calls my TakeLoan method in user.cs 
                                                        // Update available loan amount after taking the loan
            availableLoanAmount -= amount;
            DisplayMessage($"Ditt uppdaterade låneutrymme: {availableLoanAmount.ToString("C", CultureInfo.CurrentCulture)}");
        }
        
public void AmortizeLoan(User user)
{
    if (user.Loans.Count == 0)
    {
        DisplayMessage("Du har inga lån att amortera på.", true);       
        return;
    }

    user.ShowLoans();

    DisplayMessage("Ange ID för lånet du vill amortera:");
    int loanId;
    while (!int.TryParse(Console.ReadLine(), out loanId))
    {
        DisplayMessage("Ogiltig inmatning. Ange ett giltigt låne-ID.", true);
    }

    var loan = user.Loans.FirstOrDefault(l => l.LoanId == loanId); // find the loan with the matching loanID
    if (loan == null)
    {
        DisplayMessage("Lånet hittades inte.", true);
        return;
    }

    Account selectedAccount = null; 

    while (true)
    {
        DisplayMessage("Välj från vilket konto du vill amortera genom att ange konto-ID:");
        foreach (var account in user.Accounts) // Shows the user's accounts to choose from
        {
            DisplayMessage($"Konto-ID: {account.AccountID} - {account.AccountName} - Saldo: {account.Balance:C}");
        }

        int accountId;
        if (int.TryParse(Console.ReadLine(), out accountId))
        {
            selectedAccount = user.Accounts.FirstOrDefault(a => a.AccountID == accountId); // find the account with the matching accountID
            if (selectedAccount != null)
            {
                break; // Konto hittat och valt
            }
        }
        DisplayMessage("Ogiltigt konto-ID. Försök igen.", true);
    }

    decimal amount;
    while (true)
    {
        DisplayMessage("Ange amorteringsbelopp:");
        if (decimal.TryParse(Console.ReadLine(), out amount) && amount > 0)
        {
            if (amount > selectedAccount.Balance) // check if the amount is greater than the balance on selected account
            {
                DisplayMessage($"Ditt saldo för det valda kontot ({selectedAccount.AccountName}) är: {selectedAccount.Balance:C}", true);
                DisplayMessage($"Beloppet {amount:C} överstiger ditt saldo på {selectedAccount.Balance:C}.", true);
                DisplayMessage("Försök igen.", true);
            }
            else if (amount > loan.RemainingBalance) // check if the amount is greater than the remaining balance on the loan
            {
                DisplayMessage($"Du kan inte amortera mer än det återstående beloppet på ditt lån. Återstående saldo: {loan.RemainingBalance:C}", true);
                DisplayMessage("Försök igen.", true);
            }
            else
            {
                break; // Beloppet är giltigt och tillräckligt
            }
        }
        else
        {
            DisplayMessage("Ogiltig inmatning. Ange ett positivt belopp.", true);
        }
    }
    user.MakeAmortization(loan, selectedAccount, amount); // Calls the user's MakeAmortization method
    loan.MakePayment(amount); // Calls the loan's MakePayment method
    DisplayAmortizationDetails(amount, selectedAccount, loan); // Displays the details of the amortization
    if (loan.RemainingBalance == 0) // if remaning balance on the loan is 0 remove from the list and tell user it is paid.
    {
        DisplayMessage("Lånet är nu avbetalt och tas bort från dig.");
        user.Loans.Remove(loan); // Removes the loan from the user's list of loans
    }
}

private void DisplayAmortizationDetails(decimal amount, Account account, Loan loan)
{
    Console.WriteLine($"Amortering på {amount:C} har genomförts från kontot {account.AccountName}. Återstående saldo på lånet: {loan.RemainingBalance:C}");
}
// Displays a message to the user with an option to display an error message in red. Fun to try out and minimize ConsoleWriteline
public void DisplayMessage(string message, bool isError = false)
{
    if (isError)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }
    else
    {
        Console.WriteLine(message);
    }
}
        public void Return()
        {
            Console.WriteLine("Tryck Enter för att komma tillbaka till menyn.");
            Console.ReadLine();
        }

    }
}
