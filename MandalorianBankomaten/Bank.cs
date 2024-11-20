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

        public void Run()
        {
            bool loginSuccesfull = LogIn();
        }

        public bool LogIn()
        {
            int attempts = 0;
            while (attempts < 3)
            {
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
                        return true;
                    }
                }
                Console.WriteLine();
                Console.WriteLine("Inloggning misslyckades, försök igen!");
            }
            return false;
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
