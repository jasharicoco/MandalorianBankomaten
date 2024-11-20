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
            }
        }

        public bool LogIn()
        {
            int attempts = 0;
            while (attempts < 3)
            {
                const int maxAttempts = 3; // constant for max attempts allowed 
                attempts++;
                Console.Write("Vänligen skriv in ditt användernamn: ");
                string username = Console.ReadLine().ToLower();
                Console.Write("Vänligen skriv in ditt lösenord: ");
                string userpswd = ReadPassword();

                foreach (var user in users)
                {
                    if(username == user.Name && userpswd == user.Password)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Inloggning lyckades!");
                        currentUser = user;
                        return true;
                    }
                }
                Console.WriteLine();
                Console.WriteLine($"Inloggning misslyckades, försök kvar: {maxAttempts - attempts}"); // show remaining attempts
            }
            return false;
        }
        
        private void ShowUserAccounts()
        {
            currentUser.ShowAccounts(); // show the active user's accounts
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
