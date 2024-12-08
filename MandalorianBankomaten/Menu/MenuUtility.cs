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
        public static (int, ConsoleKey) ShowMenu(string[] menu, int index, ConsoleKey key, bool menuRunning)
        {
            Console.Clear();
            Console.ResetColor();
            SplitBackground(Console.WindowHeight, 45, ConsoleColor.Gray, ConsoleColor.Black);

            string title = "M A N D A L O R I A N\n";
            Console.ForegroundColor = ConsoleColor.Black; // Set text color to black
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.SetCursorPosition((45 - title.Length) / 2, 1); // Center the title in the gray section
            Console.Write(title);

            int startRow = (Console.WindowHeight - menu.Length * 2) / 2; // Vertically center the menu
            for (int i = 0; i < menu.Length; i++)
            {
                int startCol = (45 - menu[i].Length - 2) / 2;
                Console.SetCursorPosition(startCol, startRow + i * 2); // Add some padding to the left for aesthetics

                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write($"◆ {menu[i]}"); 
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write($"  {menu[i]}");
                }
            }
            MenuUtility.SpaceBackground();
            //so there's no pause for a keypress when this method is used inside the menu options
            if (menuRunning == true)
            {
                key = Console.ReadKey().Key; 
            }

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
            for (int y = 0; y < height; y++)
            {
                Console.SetCursorPosition(0, y);

                Console.BackgroundColor = leftColor;
                Console.Write(new string(' ', splitWidth));

                Console.BackgroundColor = rightColor;
                Console.Write(new string(' ', Console.WindowWidth - splitWidth));
            }

            Console.ResetColor();
        }
        public static void SpaceBackground()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            int width = Console.WindowWidth - 45;
            int height = Console.WindowHeight;
            Random random = new Random();
            int randomX;
            int randomY;
            int randomStar;
            string[] stars = { ".", "·", "•" };

            for (int i = 0; i < 30; i++)
            {
                randomStar = random.Next(0,3);
                randomX = random.Next(45, width + 45);
                randomY = random.Next(0, height);
                Console.SetCursorPosition(randomX, randomY);
                Console.Write($"{stars[randomStar]}");
            }
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
