using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace MandalorianBankomaten.Menu
{
    public class MenuUtility
    {
        // Methods
        public static void ShowBlockedMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            TextBox("❌ Du har gjort för många misslyckade försök. Kontot är tillfälligt avstängt.");
            Console.ResetColor();
        }
        public static void ShowFailedLoginMessage(int remainingAttempts)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            TextBox("❌ Inloggning misslyckades!");
            Console.WriteLine($"\nFörsök kvar: {remainingAttempts}");
            System.Threading.Thread.Sleep(1500); // Pause program for user interaction
        }
        public static void ShowUserDontExist()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            TextBox("❌ Användare finns ej i systemet, försök igen!");
            System.Threading.Thread.Sleep(1500); // Pause program for user interaction
        }
        public static void ShowSuccessMessage(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            TextBox($"✅ Inloggning lyckades! Välkommen {userName}!");
            System.Threading.Thread.Sleep(1500); // Pause program for user interaction
        }
        public static void ShowSuccessMessageAdmin(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            TextBox($"✅ Inloggning lyckades! Välkommen admin: {userName}!");
            System.Threading.Thread.Sleep(1500); // Pause program for user interaction
        }
        public static void TextBox(string message)
        {
            Console.Clear();
            bool loopCount = true;
            for (int i = 0; i < message.Length + 7; i++)
            {
                Console.Write("=");
                if (i == message.Length + 6 && loopCount)
                {
                    Console.WriteLine($"\n|  {message}  |");
                    loopCount = false;
                    i = -1;
                }
            }
            Console.ResetColor();
        }
        // (int, ConsoleKey) means the method can return both of those types. 
        public static (int, ConsoleKey) ShowMenu(string[] menu, int index, ConsoleKey key)
        {
            Console.Clear();
            Console.ResetColor();
            // Draw the split background
            SplitBackground(Console.WindowHeight, 45, ConsoleColor.Gray, ConsoleColor.Black);

            // Print the title in black text on dark gray background
            string title = "M A N D A L O R I A N\n";
            Console.ForegroundColor = ConsoleColor.Black; // Set text color to black
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition((45 - title.Length) / 2, 1); // Center the title in the gray section
            Console.Write(title);

            int startRow = (Console.WindowHeight - menu.Length * 2) / 2; // Vertically center the menu
            for (int i = 0; i < menu.Length; i++)
            {
                int startCol = (45 - menu[i].Length - 2) / 2;
                Console.SetCursorPosition(startCol, startRow + i*2); // Add some padding to the left for aesthetics

                if (i == index)
                {
                    // Highlight the selected item: dark green text on dark gray background
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write($"◆ {menu[i]}"); // Add a marker to highlight
                }
                else
                {
                    // Normal menu items: black text on dark gray background
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write($"  {menu[i]}"); // Indent unselected items for clarity
                }
            }

            Console.ResetColor();

            // Capture keypress
            key = Console.ReadKey(true).Key; // Prevent keypress from appearing in the console

            // Adjust the selected index
            if (key == ConsoleKey.UpArrow)
            {
                index = (index - 1 + menu.Length) % menu.Length; // Move up, wrap around
            }
            else if (key == ConsoleKey.DownArrow)
            {
                index = (index + 1) % menu.Length; // Move down, wrap around
            }

            return (index, key);
        }

        public static void SplitBackground(int height, int splitWidth, ConsoleColor leftColor, ConsoleColor rightColor)
        {
            // Loop through each row from the top of the console to the bottom
            for (int y = 0; y < height; y++)
            {
                // Set the cursor position at the start of the row
                Console.SetCursorPosition(0, y);

                // Set the left color and print the left part
                Console.BackgroundColor = leftColor;
                Console.Write(new string(' ', splitWidth));

                // Set the right color and print the right part
                Console.BackgroundColor = rightColor;
                Console.Write(new string(' ', Console.WindowWidth - splitWidth));
            }

            // Reset the background color after drawing
            Console.ResetColor();
        }

        public static void ASCIIArt()
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
        public static void ASCIIArtTwo()
        {
            Console.WriteLine(
                "                 ....:::::--====-:..                                                      \r\n" +
                "            ..:=++***#++++*+=====+++-:                                                    \r\n" +
                "          .-**++++=:-+*#++++*+=+=++****.                                    .::--=-:..    \r\n" +
                "         .+#%%#*===+--+*#*++*#***#**###:                             .:-=++++===++=+**=:. \r\n" +
                "         -%@@%*#+=+*#+*#%%*++##*****#*:              ....:::--===++++****==+*#*+=+*++**#+.\r\n" +
                "         =@@@%####**#**#%%##%%*+=++++++=---:::-===+++*******#%##*+**++=--=+==+#%#**######=\r\n" +
                "         .#@@@%@###*#**%%%%%#####*******+***###***##**+++******++*##%%#+--=*####%%#*#####+\r\n" +
                "          .+%%%%#####%%%%##*++++++++*****#%%#*+++++++++*+++++**=+%%%%%%@%***#%###%#**####-\r\n" +
                "           :**#%%%%%%%%%#*+++++++++**+++++*++====----=++++++++**#@%%%%**###*#%##%%#*####=.\r\n" +
                "               .:--=-:..:=*++**+=---=+**++===-------------=---=*%@@%%%##%%#######%####*:  \r\n" +
                "                      .:=+*++=+=-==+**+=====:::::--------=---=+#%%%@%%%@%####%%%%#*+-.    \r\n" +
                "                   .:=+++++-::-*###*+===--:::::-----======++++*%*=#%%%%%###%%#%%+:        \r\n" +
                "                .:=+++*+==+*+**##*+==-----=++*######***#*######+. .*###%%%%%#+-.          \r\n" +
                "             .:=+**+=+++***###*++-:.::=+*######**##*********##-      .:--=-.              \r\n" +
                "          .-=+****+=++*####**+-::-=**####******###*******##**:                            \r\n" +
                "        .*%#*%#####%##*#**+===*###**##########%%#*****#####+.                             \r\n" +
                "      .::-+#-#%%##%%#####**%%@%*%#####%####*##%%##*#%%%####-                              \r\n" +
                "   .:+#%%#%%=**#######%%%%%%@%%**#**###***#*##*#%#%%######+. .                            \r\n" +
                "==-==**++=-=***#%#%%%%%%%#=-+#@%%**####**####*#%%###*##*=.                                \r\n" +
                ":.         -+++#%#######%%%#*#%%############%%%####+=:.                                   \r\n" +
                "           -*+*#%###*#%%%########%#%########%#+=-..                                       \r\n" +
                "           :+**#%###############%#%###*=-.  .                                             \r\n" +
                "           :=**#%##############%%*=-:. ..                                                 \r\n" +
                "           .+###%#%###%######*=:..                                                        \r\n" +
                "            :*#%%####%##*+-..                                                             \r\n");

        }
    }
}
