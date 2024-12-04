namespace MandalorianBankomaten.Helpers
{
    internal class Helper
    {
        // Method to hide password input during login
        internal static string ReadPassword()
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
