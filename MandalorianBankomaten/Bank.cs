using System.Text;

namespace MandalorianBankomaten
{
    public class Bank
    {
        public List<User> users = new List<User>
        {
            new User("viggo", "1234"),
            new User("egzon", "1234"),
            new User("alex", "1234"),
            new User("margarita", "1234"),
            new User("tim", "1234"),
            new User("arbunit", "1234"),
        };

        private List<Admin> admins = new List<Admin>
        {
            new Admin("admin", "0000")
        };
        private User currentUser; // tracks the current user
        private Admin currentAdmin;
        private int fromIndex;
        private int toIndex;

        public Bank()
        {
            foreach (var user in users)
            {
                // create some standard accounts for each user
                user.AddAccount(new Account("Lönekonto", 10000, "SEK"));
                user.AddAccount(new Account("Sparkonto", 50000, "SEK"));
            }
        }

        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8; // Gör ovanliga symboler presentabla i programmet
            Ascii();
            bool programRunning = true;
            string? choice;

            Console.WriteLine("\ud83c\udf1f Välkommen till Mandalorian Bankomaten \ud83c\udf1f\n");

            bool loginSuccesfull = LogIn();
            if (loginSuccesfull) // if login is successful
            {
                while (programRunning)
                {
                    if (currentAdmin != null)
                    {
                        while (programRunning)
                        {
                            string adminMenu = "------ Menu ------\n" +
                                               "1. Skapa användare\n" +
                                               "2. Radera användare\n" +
                                               "3. Logga ut\n" +
                                               "Ditt val: ";
                            DisplayMenu(adminMenu); // Visa ASCII-konst och menyn

                            choice = Console.ReadLine();

                            switch (choice)
                            {
                                case "1":
                                    users = currentAdmin.CreateUser(users);
                                    break;
                                case "2":
                                    users = currentAdmin.DeleteUser(users);
                                    break;
                                case "3":
                                    programRunning = false;
                                    break;
                                default:
                                    Console.WriteLine("Ogiltligt menyval. Försök igen!");
                                    break;
                            }
                        }
                    }
                    else
                    {
                        while (programRunning)
                        {
                            string userMenu = "------ Menu ------\n" +
                                              "1. Visa konton\n" +
                                              "2. Lägg till konto\n" +
                                              "3. Ta bort konto\n" +
                                              "4. För över pengar mellan konton\n" +
                                              "5. För över pengar till en annan användare\n" +
                                              "6. Logga ut\n" +
                                              "Ditt val: ";
                            DisplayMenu(userMenu); // Visa ASCII-konst och menyn

                            choice = Console.ReadLine();

                            switch (choice)
                            {
                                case "1":
                                    currentUser.ShowAccounts();
                                    Return();
                                    break;
                                case "2":
                                    currentUser.CreateAccount();
                                    Return();
                                    break;
                                case "3":
                                    //currentUser.RemoveAccount();
                                    Return();
                                    break;
                                case "4":
                                    TransferBetweenAccounts();
                                    Return();
                                    break;
                                case "5":
                                    TransferToAnotherUser();
                                    Return();
                                    break;
                                case "6":
                                    programRunning = false;
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
            if (!int.TryParse(Console.ReadLine(), out int fromIndex) || fromIndex < 1 || fromIndex > currentUser.Accounts.Count)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }
            var fromAccount = currentUser.Accounts[fromIndex - 1];

            // Välj mottagarkonto
            Console.Write("Ange numret för kontot att överföra till: ");
            if (!int.TryParse(Console.ReadLine(), out int toIndex) || toIndex < 1 || toIndex > currentUser.Accounts.Count || toIndex == fromIndex)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }
            var toAccount = currentUser.Accounts[toIndex - 1];

            // Ange belopp
            Console.Write("Ange belopp att överföra: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Beloppet måste vara ett positivt tal.");
                return;
            }

            // Utför överföringen
            currentUser.TransferMoneyBetweenAccounts(fromAccount, toAccount, amount).Wait();
        }

        public void TransferToAnotherUser()
        {
            // Visa alla användare för att välja mottagare
            Console.WriteLine("\nTillgängliga användare:");
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i] != currentUser) // Visa inte den inloggade användaren som val
                {
                    Console.WriteLine($"{i + 1}. {users[i].Name}");
                }
            }

            Console.Write("\nAnge numret för mottagaren: ");
            if (!int.TryParse(Console.ReadLine(), out int recipientIndex) || recipientIndex < 1 || recipientIndex > users.Count || users[recipientIndex - 1] == currentUser)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }
            var recipient = users[recipientIndex - 1];

            if (!recipient.HasAccounts())
            {
                return;
            }

            // Välj avsändarkonto
            Console.WriteLine("\nDina konton:");
            for (int i = 0; i < currentUser.Accounts.Count; i++)
            {
                var account = currentUser.Accounts[i];
                Console.WriteLine($"{i + 1}. {account.AccountName} - Saldo: {account.Balance:C}");
            }

            Console.Write("Ange numret för kontot att överföra från: ");
            if (!int.TryParse(Console.ReadLine(), out int fromIndex) || fromIndex < 1 || fromIndex > currentUser.Accounts.Count)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }
            var fromAccount = currentUser.Accounts[fromIndex - 1];

            // Välj mottagarens konto
            Console.WriteLine($"\nKonton för {recipient.Name}:");
            for (int i = 0; i < recipient.Accounts.Count; i++)
            {
                var account = recipient.Accounts[i];
                Console.WriteLine($"{i + 1}. {account.AccountName} - Saldo: {account.Balance:C}");
            }

            Console.Write("Ange numret för mottagarens konto: ");
            if (!int.TryParse(Console.ReadLine(), out int toIndex) || toIndex < 1 || toIndex > recipient.Accounts.Count)
            {
                Console.WriteLine("Ogiltigt val.");
                return;
            }
            var recipientAccount = recipient.Accounts[toIndex - 1];

            // Ange belopp
            Console.Write("Ange belopp att överföra: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Beloppet måste vara ett positivt tal.");
                return;
            }

            // Utför överföringen
            currentUser.TransferMoneyToUser(recipient, fromAccount, recipientAccount, amount).Wait();
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
        public void TakeLoan()
        {
            if (currentUser == null)
            {
                Console.WriteLine("Du måste vara inloggad för att ta ett lån.");
                return;
            }

            Console.Write("Ange lånebelopp: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Ogiltigt belopp.");
                return;
            }

            Console.Write("Ange ränta (i procent): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal interestRate) || interestRate <= 0)
            {
                Console.WriteLine("Ogiltig ränta.");
                return;
            }

            currentUser.TakeLoan(interestRate, amount);
        }

        // Method to show users loans
        public void ShowLoans()
        {
            if (currentUser == null)
            {
                Console.WriteLine("Du måste vara inloggad för att se lån.");
                return;
            }
            currentUser.ShowLoans();
        }
        public void Ascii()
        {
            Console.WriteLine("⠀⢀⣠⣄⣀⣀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣤⣴⣶⡾⠿⠿⠿⠿⢷⣶⣦⣤⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⢰⣿⡟⠛⠛⠛⠻⠿⠿⢿⣶⣶⣦⣤⣤⣀⣀⡀⣀⣴⣾⡿⠟⠋⠉⠀⠀⠀⠀⠀⠀⠀⠀⠉⠙⠻⢿⣷⣦⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⣀⣀⣀⣀⣀⡀\r\n⠀⠻⣿⣦⡀⠀⠉⠓⠶⢦⣄⣀⠉⠉⠛⠛⠻⠿⠟⠋⠁⠀⠀⠀⣤⡀⠀⠀⢠⠀⠀⠀⣠⠀⠀⠀⠀⠈⠙⠻⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠟⠛⠛⢻⣿\r\n⠀⠀⠈⠻⣿⣦⠀⠀⠀⠀⠈⠙⠻⢷⣶⣤⡀⠀⠀⠀⠀⢀⣀⡀⠀⠙⢷⡀⠸⡇⠀⣰⠇⠀⢀⣀⣀⠀⠀⠀⠀⠀⠀⣀⣠⣤⣤⣶⡶⠶⠶⠒⠂⠀⠀⣠⣾⠟\r\n⠀⠀⠀⠀⠈⢿⣷⡀⠀⠀⠀⠀⠀⠀⠈⢻⣿⡄⣠⣴⣿⣯⣭⣽⣷⣆⠀⠁⠀⠀⠀⠀⢠⣾⣿⣿⣿⣿⣦⡀⠀⣠⣾⠟⠋⠁⠀⠀⠀⠀⠀⠀⠀⣠⣾⡟⠁⠀\r\n⠀⠀⠀⠀⠀⠈⢻⣷⣄⠀⠀⠀⠀⠀⠀⠀⣿⡗⢻⣿⣧⣽⣿⣿⣿⣧⠀⠀⣀⣀⠀⢠⣿⣧⣼⣿⣿⣿⣿⠗⠰⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⣠⣾⡿⠋⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠙⢿⣶⣄⡀⠀⠀⠀⠀⠸⠃⠈⠻⣿⣿⣿⣿⣿⡿⠃⠾⣥⡬⠗⠸⣿⣿⣿⣿⣿⡿⠛⠀⢀⡟⠀⠀⠀⠀⠀⠀⣀⣠⣾⡿⠋⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠛⠿⣷⣶⣤⣤⣄⣰⣄⠀⠀⠉⠉⠉⠁⠀⢀⣀⣠⣄⣀⡀⠀⠉⠉⠉⠀⠀⢀⣠⣾⣥⣤⣤⣤⣶⣶⡿⠿⠛⠉⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⢻⣿⠛⢿⣷⣦⣤⣴⣶⣶⣦⣤⣤⣤⣤⣬⣥⡴⠶⠾⠿⠿⠿⠿⠛⢛⣿⣿⣿⣯⡉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣿⣧⡀⠈⠉⠀⠈⠁⣾⠛⠉⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣴⣿⠟⠉⣹⣿⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣸⣿⣿⣦⣀⠀⠀⠀⢻⡀⠀⠀⠀⠀⠀⠀⠀⢀⣠⣤⣶⣿⠋⣿⠛⠃⠀⣈⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⡿⢿⡀⠈⢹⡿⠶⣶⣼⡇⠀⢀⣀⣀⣤⣴⣾⠟⠋⣡⣿⡟⠀⢻⣶⠶⣿⣿⠛⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⣿⣷⡈⢿⣦⣸⠇⢀⡿⠿⠿⡿⠿⠿⣿⠛⠋⠁⠀⣴⠟⣿⣧⡀⠈⢁⣰⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⢻⣦⣈⣽⣀⣾⠃⠀⢸⡇⠀⢸⡇⠀⢀⣠⡾⠋⢰⣿⣿⣿⣿⡿⠟⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠿⢿⣿⣿⡟⠛⠃⠀⠀⣾⠀⠀⢸⡇⠐⠿⠋⠀⠀⣿⢻⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⠁⢀⡴⠋⠀⣿⠀⠀⢸⠇⠀⠀⠀⠀⠀⠁⢸⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣿⡿⠟⠋⠀⠀⠀⣿⠀⠀⣸⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣁⣀⠀⠀⠀⠀⣿⡀⠀⣿⠀⠀⠀⠀⠀⠀⢀⣈⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀\r\n⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠛⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠟⠛⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
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
