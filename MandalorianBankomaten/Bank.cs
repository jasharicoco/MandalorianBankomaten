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
        private User currentUser; // tracks the current user
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
            bool loginSuccesfull = LogIn();
            if (loginSuccesfull) // if login is successful
            {
                ShowUserAccounts(); // show the user's accounts
                // här kommer menyn att vara så småningom

                //testkör metoder
                //TransferToAnotherUser();
                TransferBetweenAccounts();
            }
        }

        public bool LogIn()
        {
            int attempts = 0;
            const int maxAttempts = 3; // constant for max attempts allowed 
            do
            {
                attempts++;
                Console.Write("Vänligen skriv in ditt användernamn: ");
                string username = Console.ReadLine().ToLower();
                Console.Write("Vänligen skriv in ditt lösenord: ");
                string userpswd = ReadPassword();

                foreach (var user in users)
                {
                    if (username == user.Name && userpswd == user.Password)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Inloggning lyckades!");
                        currentUser = user;
                        return true;
                    }
                }
                Console.WriteLine();
                Console.WriteLine($"Inloggning misslyckades, försök kvar: {maxAttempts - attempts}"); // show remaining attempts
            } while (attempts < 3);
            Console.WriteLine("Inga försök kvar. Du är tillfälligt avstängd.");
            return false;
        }

        private void ShowUserAccounts()
        {
            currentUser.ShowAccounts(); // show the active user's accounts
        }

        public void TransferBetweenAccounts()
        {
            // Visa användarens konton
            Console.WriteLine("\nDina konton:");
            for (int i = 0; i < currentUser.Accounts.Count; i++)
            {
                var account = currentUser.Accounts[i];
                Console.WriteLine($"{i + 1}. {account.AccountName} - Saldo: {account.Balance:C}");
            }

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
            currentUser.TransferMoneyBetweenAccounts(fromAccount, toAccount, amount);
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
            currentUser.TransferMoneyToUser(recipient, fromAccount, recipientAccount, amount);
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
    }
}
