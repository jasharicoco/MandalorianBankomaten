using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.ComponentModel.Design;

namespace MandalorianBankomaten
{
    public class Bank
    {
        private List<User> users;
        private List<Admin> admins;
        private User currentUser; // tracks the current user
        private Admin currentAdmin;
        private TransactionLog transactionLog;


        public Bank()
        {
            // Initialize users and admins from AccountSeeder
            users = Seeder.SeedUsers();
            admins = Seeder.SeedAdmins();
            // initialize log and create a log-file
            transactionLog = new TransactionLog("transaction.log");
            // Add standard accounts to each user
            Seeder.AddStandardAccountsToUsers(users);
        }

        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8; // Gör ovanliga symboler synliga i programmet
            Ascii();
            bool programRunning = true;
            string? choice;

            Console.WriteLine("\ud83c\udf1f Välkommen till Mandalorian Bankomaten \ud83c\udf1f\n");

            bool loginSuccesfull = LogIn();
            if (loginSuccesfull) // if login is successful
            {
                while (programRunning)
                {
                    Console.Clear();
                    //menyvalen indexas. En for-loop går igenom menyn och skriver ut den rad som indexet står på med en färg,
                    //och skriver ut resten utan färg. 
                    string[] menu = {"1. Visa konton\n" ,
                        "2. Lägg till konto\n" ,
                        "3. Ta bort konto\n" ,
                        "4. För över pengar mellan konton\n" ,
                        "5. För över pengar till en annan användare\n" ,
                        "6. Ta lån\n" ,
                        "7. Logga ut\n" };
                    int choiceIndex = 0;

                    if (currentAdmin != null)
                    {
                        string[] adminMenu = {"1. Skapa användare\n" ,
                                               "2. Radera användare\n" ,
                                               "3. Logga ut\n" };

                        int adminChoiceIndex = 0;

                        while (programRunning)
                        {
                            Console.Clear();
                            Console.WriteLine("  ------ Menu -------\n");
                            for (int i = 0; i < adminMenu.Length; i++)
                            {
                                //i starts at 0 and is the same value as choiceIndex -> writes that line in blue
                                if (i == adminChoiceIndex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine($"◆ {adminMenu[i]}");
                                    Console.ResetColor();
                                }
                                //Writes the other ones like normal
                                else
                                {
                                    Console.WriteLine($"  {adminMenu[i]}");
                                }
                                
                            }
                            //logs keypress
                            ConsoleKey key = Console.ReadKey().Key;

                            //if key is up arrow, lower the value of choiceIndex by 1. If it goes below 0 it becomes out of bounds
                            //so in that case it turns into the highest index, meaning it goes to the bottom of the list in the menu. 
                            if (key == ConsoleKey.UpArrow)
                            {
                                adminChoiceIndex = adminChoiceIndex - 1;
                                if (adminChoiceIndex < 0)
                                {
                                    adminChoiceIndex = adminMenu.Length - 1;
                                }
                            }
                            else if (key == ConsoleKey.DownArrow)
                            {
                                adminChoiceIndex = adminChoiceIndex + 1;
                                if (adminChoiceIndex == adminMenu.Length)
                                {
                                    adminChoiceIndex = 0;
                                }
                            }
                            else if (key == ConsoleKey.Enter)
                            {
                                //adminChoiceIndex is 0 when the first option is highlighted, hence the + 1. 
                                switch (adminChoiceIndex + 1)
                                {
                                    case 1:
                                        users = currentAdmin.CreateUser(users);
                                        break;
                                    case 2:
                                        users = currentAdmin.DeleteUser(users);
                                        break;
                                    case 3:
                                        currentAdmin = null;
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
                        while (programRunning)
                        {
                            Console.Clear();
                            Console.WriteLine("  ------ Menu -------\n");
                            for (int i = 0; i < menu.Length; i++)
                            {
                                if (i == choiceIndex)
                                {
                                    Console.ForegroundColor = ConsoleColor.Blue;
                                    Console.WriteLine($"◆ {menu[i]}");
                                    Console.ResetColor();
                                }
                                else
                                {
                                    Console.WriteLine($"  {menu[i]}");
                                }
                            }

                            ConsoleKey key = Console.ReadKey().Key;

                            if (key == ConsoleKey.UpArrow)
                            {
                                choiceIndex = choiceIndex - 1;
                                if (choiceIndex < 0)
                                {
                                    choiceIndex = menu.Length - 1;
                                }
                            }
                            else if (key == ConsoleKey.DownArrow)
                            {
                                choiceIndex = choiceIndex + 1;
                                if (choiceIndex == menu.Length)
                                {
                                    choiceIndex = 0;
                                }
                            }
                            else if (key == ConsoleKey.Enter)
                            {
                                Console.Clear();
                                switch (choiceIndex + 1)
                                {
                                    case 1:
                                        currentUser.ShowAccounts();
                                        Return();
                                        break;
                                    case 2:
                                        currentUser.CreateAccount();
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
                                        currentUser = null;
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
            const int maxAttempts = 3; // constant for max attempts allowed 
            do
            {
                attempts++;
                Console.Write("Vänligen skriv in ditt \ud83d\udc64 användernamn: ");
                string username = Console.ReadLine().ToLower();
                Console.Write("Vänligen skriv in ditt \ud83d\udd12 lösenord: ");
                string userpswd = ReadPassword();

                foreach (var admin in admins)
                {
                    if (username == admin.Name && userpswd == admin.Password)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Inloggad som admin");
                        currentAdmin = admin;
                        return true;
                    }
                }

                foreach (var user in users)
                {
                    if (username == user.Name && userpswd == user.Password)
                    {
                        Console.WriteLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n====================================================");
                        Console.WriteLine($"\t✅ Inloggning lyckades! Välkommen {currentUser}!");
                        Console.WriteLine("====================================================");
                        Console.ResetColor();
                        System.Threading.Thread.Sleep(1500); // Vänta för att visa meddelandet
                        currentUser = user;
                        return true;
                    }
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n====================================================");
                Console.WriteLine("\t   ❌ Inloggning misslyckades!");
                Console.WriteLine("====================================================");
                Console.ResetColor();
                Console.WriteLine($"\tFörsök kvar: {maxAttempts - attempts}");
                System.Threading.Thread.Sleep(1500); // delay for next try
            } while (attempts < 3);
            
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("\n====================================================");
            Console.WriteLine("❌ Du har gjort för många misslyckade försök. Kontot är tillfälligt avstängt.");
            Console.WriteLine("====================================================");
            Console.ResetColor();
            return false;
        }

        public void TransferBetweenAccounts()
        {
            // Visa användarens konton
            currentUser.ShowAccounts();

            // Välj avsändarkonto
            Console.Write("\nAnge numret för kontot att överföra från: ");
            if (!int.TryParse(Console.ReadLine(), out int fromId) || fromId < 4850)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }

            // Find the account with the matching accountID
            var fromAccount = currentUser.Accounts.FirstOrDefault(account => account.AccountID == fromId);

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
            var toAccount = currentUser.Accounts.FirstOrDefault(account => account.AccountID == toId);

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
            currentUser.TransferMoneyBetweenAccounts(fromAccount, toAccount, amount);

            // Log the transaction----------
            string transferInfo = $"Överföring: {amount} från konto {fromAccount} till konto {toAccount}";
            transactionLog.LogTransaction(transferInfo);
        }

        public void TransferToAnotherUser()
        {
            // Visa användarens konton för att välja avsändarkonto
            currentUser.ShowAccounts();

            Console.Write("Ange numret för kontot att överföra från: ");
            if (!int.TryParse(Console.ReadLine(), out int fromId) || fromId < 4850)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }

            // Hitta avsändarkontot med matchande AccountID
            var fromAccount = currentUser.Accounts.FirstOrDefault(account => account.AccountID == fromId);
            if (fromAccount == null)
            {
                Console.WriteLine("Inget konto hittades med det numret.");
                return;
            }

            var allAccounts = new List<Account>();
            foreach (var user in users)
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
            var recipient = users.FirstOrDefault(user => user.Accounts.Contains(recipientAccount));
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
            currentUser.TransferMoneyToAccount(fromAccount, recipientAccount, amount);

            // Log the transaction----------
            string transactionInfo = $"Överföring: {amount} från konto {fromAccount} till konto {recipientAccount}";
            transactionLog.LogTransaction(transactionInfo);

            // Bekräftelse av överföringen
            Console.WriteLine($"\nDu har skickat {amount:C} från konto {fromAccount.AccountID}: {fromAccount.AccountName} till konto {recipientAccount.AccountID}.");
            Console.WriteLine($"Ditt nya saldo är för konto {fromAccount.AccountID}: {fromAccount.AccountName} är: {fromAccount.Balance:C}.");
        }

        static string ReadPassword()
        {
            string password = string.Empty;

            while (true)
            {
                // Read a single character
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                // Check if the key is Enter
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    break;
                }
                // Check if the key is Backspace
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        // Remove the last character from the password
                        password = password[..^1];
                        // Move the cursor back, overwrite the '*' and move back again
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    // Add the character to the password
                    password += keyInfo.KeyChar;
                    // Display an asterisk
                    Console.Write('*');
                }
            }
            return password;
        }

        // Method to offer a loan to user
    public void TakeLoan() // programmed by Alex & Tim
    {
        if (currentUser == null)
        {
            Console.WriteLine("Du måste vara inloggad för att ta ett lån.");
            return;
        }
        Console.WriteLine($"Hej {currentUser.Name}! Välkommen till bankens låneavdelning. Du kan låna upp till 5 gånger ditt totala saldo.");
        decimal maxLoanAmount = currentUser.Accounts.Sum(account => account.Balance) * 5; 
        decimal currentLoanAmount = currentUser.Loans.Sum(loan => loan.RemainingBalance);
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
        
        currentUser.TakeLoan(amount, loanCategory); // Calls my TakeLoan method in user.cs 
        // Update available loan amount after taking the loan
        availableLoanAmount -= amount;
        Console.WriteLine($"Ditt uppdaterade låneutrymme: {availableLoanAmount.ToString("C", CultureInfo.CurrentCulture)}");
}

        // Method to show users loans but as of now our program shows this in the ShowAccounts method do we really need this?
        /*public void ShowLoans()
        {
            if (currentUser == null)
            {
                Console.WriteLine("Du måste vara inloggad för att se lån.");
                return;
            }
            currentUser.ShowLoans();
        }
        */
        public void Ascii()
        {
            Console.WriteLine(
                "⠀⢀⣠⣄⣀⣀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣤⣴⣶⡾⠿⠿⠿⠿⢷⣶⣦⣤⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⢰⣿⡟⠛⠛⠛⠻⠿⠿⢿⣶⣶⣦⣤⣤⣀⣀⡀⣀⣴⣾⡿⠟⠋⠉⠀⠀⠀⠀⠀⠀⠀⠀⠉⠙⠻⢿⣷⣦⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⣀⣀⣀⣀⣀⡀\r\n" +
                "⠀⠻⣿⣦⡀⠀⠉⠓⠶⢦⣄⣀⠉⠉⠛⠛⠻⠿⠟⠋⠁⠀⠀⠀⣤⡀⠀⠀⢠⠀⠀⠀⣠⠀⠀⠀⠀⠈⠙⠻⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠟⠛⠛⢻⣿\r\n" +
                "⠀⠀⠈⠻⣿⣦⠀⠀⠀⠀⠈⠙⠻⢷⣶⣤⡀⠀⠀⠀⠀⢀⣀⡀⠀⠙⢷⡀⠸⡇⠀⣰⠇⠀⢀⣀⣀⠀⠀⠀⠀⠀⠀⣀⣠⣤⣤⣶⡶⠶⠶⠒⠂⠀⠀⣠⣾⠟\r\n" +
                "⠀⠀⠀⠀⠈⢿⣷⡀⠀⠀⠀⠀⠀⠀⠈⢻⣿⡄⣠⣴⣿⣯⣭⣽⣷⣆⠀⠁⠀⠀⠀⠀⢠⣾⣿⣿⣿⣿⣦⡀⠀⣠⣾⠟⠋⠁⠀⠀⠀⠀⠀⠀⠀⣠⣾⡟⠁⠀\r\n" +
                "⠀⠀⠀⠀⠀⠈⢻⣷⣄⠀⠀⠀⠀⠀⠀⠀⣿⡗⢻⣿⣧⣽⣿⣿⣿⣧⠀⠀⣀⣀⠀⢠⣿⣧⣼⣿⣿⣿⣿⠗⠰⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⣠⣾⡿⠋⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠙⢿⣶⣄⡀⠀⠀⠀⠀⠸⠃⠈⠻⣿⣿⣿⣿⣿⡿⠃⠾⣥⡬⠗⠸⣿⣿⣿⣿⣿⡿⠛⠀⢀⡟⠀⠀⠀⠀⠀⠀⣀⣠⣾⡿⠋⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠛⠿⣷⣶⣤⣤⣄⣰⣄⠀⠀⠉⠉⠉⠁⠀⢀⣀⣠⣄⣀⡀⠀⠉⠉⠉⠀⠀⢀⣠⣾⣥⣤⣤⣤⣶⣶⡿⠿⠛⠉⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⢻⣿⠛⢿⣷⣦⣤⣴⣶⣶⣦⣤⣤⣤⣤⣬⣥⡴⠶⠾⠿⠿⠿⠿⠛⢛⣿⣿⣿⣯⡉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣿⣧⡀⠈⠉⠀⠈⠁⣾⠛⠉⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣴⣿⠟⠉⣹⣿⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣸⣿⣿⣦⣀⠀⠀⠀⢻⡀⠀⠀⠀⠀⠀⠀⠀⢀⣠⣤⣶⣿⠋⣿⠛⠃⠀⣈⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⡿⢿⡀⠈⢹⡿⠶⣶⣼⡇⠀⢀⣀⣀⣤⣴⣾⠟⠋⣡⣿⡟⠀⢻⣶⠶⣿⣿⠛⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⣿⣷⡈⢿⣦⣸⠇⢀⡿⠿⠿⡿⠿⠿⣿⠛⠋⠁⠀⣴⠟⣿⣧⡀⠈⢁⣰⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⢻⣦⣈⣽⣀⣾⠃⠀⢸⡇⠀⢸⡇⠀⢀⣠⡾⠋⢰⣿⣿⣿⣿⡿⠟⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠿⢿⣿⣿⡟⠛⠃⠀⠀⣾⠀⠀⢸⡇⠐⠿⠋⠀⠀⣿⢻⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⠁⢀⡴⠋⠀⣿⠀⠀⢸⠇⠀⠀⠀⠀⠀⠁⢸⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣿⡿⠟⠋⠀⠀⠀⣿⠀⠀⣸⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣁⣀⠀⠀⠀⠀⣿⡀⠀⣿⠀⠀⠀⠀⠀⠀⢀⣈⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n" +
                "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠛⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠟⠛⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
        }
        private void DisplayMenu(string menu)
        {
            Console.Clear();
            Ascii(); // Visa ASCII-konsten
            Console.WriteLine(menu); // Skriv ut menyn under ASCII-konsten
        }

        public void Return()
        {
            Console.WriteLine("Tryck Enter för att komma tillbaka till menyn.");
            Console.ReadLine();
        }
    }
}
