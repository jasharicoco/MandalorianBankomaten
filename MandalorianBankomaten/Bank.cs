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

            Console.WriteLine("\ud83c\udf1f Välkommen till Mandalorian Bankomaten \ud83c\udf1f\n");

            bool loginSuccesfull = LogIn();
            if (loginSuccesfull) // if login is successful
            {
                while (programRunning)
                {
                    MenuUtility.ColorScheme();
                    ConsoleKey key = ConsoleKey.A;
                    int choiceIndex = 0;
                    //menyvalen indexas. En for-loop går igenom menyn och skriver ut den rad som indexet står på med en färg,
                    //och skriver ut resten utan färg.
                    string[] menu = {"Visa konton\n" ,
                        "Lägg till konto\n" ,
                        "Ta bort konto\n" ,
                        "För över pengar mellan konton\n" ,
                        "För över pengar till en annan användare\n" ,
                        "Ta lån\n" ,
                        "Logga ut\n" };

                    if (CurrentAdmin != null)
                    {
                        string[] adminMenu = {"Skapa användare\n" ,
                                               "Radera användare\n" ,
                                               "Logga ut\n" };

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
                Console.Write("Vänligen skriv in ditt \ud83d\udc64 användernamn: ");
                string username = Console.ReadLine().ToLower();
                Console.Write("Vänligen skriv in ditt \ud83d\udd12 lösenord: ");
                string userpswd = Helper.ReadPassword();

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

            Console.Write("Ange numret för kontot att överföra från: ");
            if (!int.TryParse(Console.ReadLine(), out int fromId) || fromId < 4850)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }
            if (fromId >= 8540)
            {
                Console.WriteLine("Du kan ej överföra från ett lånekonto.");
                return;
            }
            // Hitta avsändarkontot med matchande AccountID
            var fromAccount = CurrentUser.Accounts.FirstOrDefault(account => account.AccountID == fromId);
            if (fromAccount == null)
            {
                Console.WriteLine("Inget konto hittades med det numret.");
                return;
            }

            var allAccounts = new List<Account>();
            foreach (var user in Users)
            {
                allAccounts.AddRange(user.Accounts);
            }

            // Ange mottagarens kontonummer
            Console.Write("Ange numret för mottagarens konto: ");
            if (!int.TryParse(Console.ReadLine(), out int toId) || toId < 4850)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }

            // Hitta mottagarens konto med matchande AccountID
            var recipientAccount = allAccounts.FirstOrDefault(account => account.AccountID == toId);
            if (recipientAccount == null)
            {
                Console.WriteLine("Inget konto hittades med det numret.");
                return;
            }

            // Hitta mottagarens användare genom att kolla vilket konto som tillhör vilken användare
            var recipient = Users.FirstOrDefault(user => user.Accounts.Contains(recipientAccount));
            if (recipient == null)
            {
                Console.WriteLine("Mottagaren finns inte.");
                return;
            }

            // Ange belopp
            Console.Write("Ange belopp att överföra: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0 || amount > fromAccount.Balance)
            {
                Console.WriteLine("Ogiltigt belopp. Beloppet måste vara positivt och mindre än eller lika med ditt saldo.");
                return;
            }

            // Utför överföringen
            CurrentUser.TransferMoneyToAccount(fromAccount, recipientAccount, amount);

            // Log the transaction----------
            string transactionInfo = $"Överföring: {amount} från konto {fromAccount} till konto {recipientAccount}";
            //TransactionLog.LogTransaction(transactionInfo);

            // Bekräftelse av överföringen
            Console.WriteLine($"\nDu har skickat {amount:C} från konto {fromAccount.AccountID}: {fromAccount.AccountName} till konto {recipientAccount.AccountID}.");
            Console.WriteLine($"Ditt nya saldo är för konto {fromAccount.AccountID}: {fromAccount.AccountName} är: {fromAccount.Balance:C}.");
        }
        public void TransferBetweenAccounts()
        {
            // Visa användarens konton
            CurrentUser.ShowAccounts();

            // Välj avsändarkonto
            Console.Write("\nAnge numret för kontot att överföra från: ");
            if (!int.TryParse(Console.ReadLine(), out int fromId) || fromId < 4850)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }

            if (fromId >= 8540)
            {
                Console.WriteLine("Du kan ej överföra från ett lånekonto.");
                return;
            }

            // Find the account with the matching accountID
            var fromAccount = CurrentUser.Accounts.FirstOrDefault(account => account.AccountID == fromId);

            if (fromAccount == null)
            {
                Console.WriteLine("Inget konto hittades med det numret.");
                return;
            }

            // Välj mottagarkonto
            Console.Write("Ange numret för kontot att överföra till: ");
            if (!int.TryParse(Console.ReadLine(), out int toId) || toId < 4850 || toId == fromId)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }
            if (toId >= 8540)
            {
                Console.WriteLine("Du kan ej överföra till ett lånekonto.");
                return;
            }
            var toAccount = CurrentUser.Accounts.FirstOrDefault(account => account.AccountID == toId);

            if (toAccount == null)
            {
                Console.WriteLine("Inget konto hittades med det numret.");
                return;
            }
            // Ange belopp
            Console.Write("Ange belopp att överföra: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Beloppet måste vara ett positivt tal.");
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
                Console.WriteLine("Du måste vara inloggad för att ta ett lån.");
                return;
            }
            Console.WriteLine($"Hej {CurrentUser.Name}! Välkommen till bankens låneavdelning. Du kan låna upp till 5 gånger ditt totala saldo.");
            decimal maxLoanAmount = CurrentUser.Accounts.Sum(account => account.Balance) * 5;
            decimal currentLoanAmount = CurrentUser.Loans.Sum(loan => loan.RemainingBalance);
            decimal availableLoanAmount = maxLoanAmount - currentLoanAmount;

            Console.WriteLine(
                $"Ditt nuvarande låneutrymme: {availableLoanAmount.ToString("C", CultureInfo.CurrentCulture)}");

            decimal amount;
            Loan.LoanCategory loanCategory;
            // input for loan type
            Console.WriteLine("Välj typ av lån:");
            Console.WriteLine("1. 🏠 Bolån 4% ränta");
            Console.WriteLine("2. 🚗 Billån 8% ränta");
            Console.WriteLine("3. 💳 Privatlån 10% ränta");
            int choice;
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= 3)
                {
                    break;
                }
                Console.WriteLine("Ogiltigt val. Försök igen.");
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
                Console.Write("Ange lånebelopp: ");
                if (!decimal.TryParse(Console.ReadLine(), out amount) || amount <= 0)
                {
                    Console.WriteLine("Ogiltigt belopp. Försök igen.");
                    continue;
                }
                if (amount + currentLoanAmount > maxLoanAmount)
                {
                    Console.WriteLine("Beloppet överstiger ditt tillgängliga låneutrymme. Försök igen.");
                }
                else if (amount > availableLoanAmount)
                {
                    Console.WriteLine("Beloppet överstiger det lånebelopp du kan ta för den valda lånetypen. Försök igen.");
                }
            } while (amount <= 0 || amount + currentLoanAmount > maxLoanAmount || amount > availableLoanAmount);

            CurrentUser.TakeLoan(amount, loanCategory); // Calls my TakeLoan method in user.cs 
                                                        // Update available loan amount after taking the loan
            availableLoanAmount -= amount;
            Console.WriteLine($"Ditt uppdaterade låneutrymme: {availableLoanAmount.ToString("C", CultureInfo.CurrentCulture)}");
        }
        public void Return()
        {
            Console.WriteLine("Tryck Enter för att komma tillbaka till menyn.");
            Console.ReadLine();
        }

    }
}
