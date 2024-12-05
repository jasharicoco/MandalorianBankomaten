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
            MenuUtility.ColorScheme();
            Console.Clear();
            string text = "M A N D A L O R I A N\n\n";
            Console.SetCursorPosition((Console.WindowWidth - text.Length + 5) / 2, Console.CursorTop + 5);
            Console.WriteLine(text);
            for (int i = 0; i < menu.Length; i++)
            {
                Console.SetCursorPosition((Console.WindowWidth - menu[i].Length) / 2, Console.CursorTop);
                //i starts at 0 and is the same value as choiceIndex -> writes that line in blue
                if (i == index)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"◆ {menu[i]}");
                    Console.ResetColor();
                }
                //Writes the other ones like normal
                else
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine($"  {menu[i]}");
                }

            }
            //logs keypress
            key = Console.ReadKey().Key;

            //if key is up arrow, lower the value of choiceIndex by 1. If it goes below 0 it goes out of bounds
            //and turns into the highest index, going to the bottom of the list in the menu. 
            if (key == ConsoleKey.UpArrow)
            {
                index = index - 1;
                if (index < 0)
                {
                    index = menu.Length - 1;
                }
            }
            else if (key == ConsoleKey.DownArrow)
            {
                index = index + 1;
                if (index == menu.Length)
                {
                    index = 0;
                }
            }
            return (index, key);
        }
        public static (ConsoleColor Foreground, ConsoleColor Background) ColorScheme()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            return (Console.ForegroundColor, Console.BackgroundColor);
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
