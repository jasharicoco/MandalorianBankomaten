namespace MandalorianBankomaten.Menu
{
    public class MenuUtility
    {
        // Methods
        public static void ShowBlockedMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            TextBox("❌ Du har gjort för många misslyckade försök. Kontot är tillfälligt avstängt.");
            Console.WriteLine();
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
            Console.WriteLine();
            System.Threading.Thread.Sleep(1500); // Pause program for user interaction
        }
        public static void ShowSuccessMessage(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            TextBox($"✅ Inloggning lyckades! Välkommen {userName}!");
            Console.WriteLine();
            System.Threading.Thread.Sleep(1500); // Pause program for user interaction
        }
        public static void ShowSuccessMessageAdmin(string userName)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            TextBox($"✅ Inloggning lyckades! Välkommen admin: {userName}");
            Console.WriteLine();
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
            Console.WriteLine();
            Console.ResetColor();
        }
        // (int, ConsoleKey) means the method can return both of those types. 
        public static (int, ConsoleKey) ShowMenu(string[] menu, int index, ConsoleKey key, bool menuRunning, int[] x, int[] y, int[] randomStar)
        {
            Console.Clear();
            Console.ResetColor();
            bool hasRun = false;
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
                    // Normal menu items: black text on dark gray background
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    Console.Write($"  {menu[i]}");
                }
            }

            SpaceBackground(randomStar, x, y, 50);

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
            Console.ResetColor();
        }
        public static void SpaceBackground(int[] randomStar, int[] x, int[] y, int starCount)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Random random = new Random();

            string[] stars = { ".", "·", "•" };
            for (int i = 0; i < starCount; i++)
            {
                Console.SetCursorPosition(x[i], y[i]);
                Console.Write($"{stars[randomStar[i]]}");
            }
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
        }
        public static (int[], int[], int[]) SpaceBackgroundPrep(int[] x, int[] y, int[] randomStar, int width, int height, int xStartingPoint, int starCount)
        {
            //int width = Console.WindowWidth - 45;
            //int height = Console.WindowHeight;
            Random random = new Random();

            for (int i = 0; i < starCount; i++)
            {
                int star = random.Next(0, 3);
                int randomX = random.Next(xStartingPoint, width + xStartingPoint);
                int randomY = random.Next(0, height);
                x[i] = randomX;
                y[i] = randomY;
                randomStar[i] = star;
            }

            return (x, y, randomStar);
        }
        public static void SplitBackground(int height, int splitWidth, ConsoleColor leftColor, ConsoleColor rightColor)
        {
            Console.Clear();
            Console.ResetColor();
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

        public static void CustomWriteLine(int fixedX, string text)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            int currentY = Console.CursorTop;
            Console.SetCursorPosition(fixedX, currentY);
            Console.WriteLine(text);
        }
        public static void CustomWriteLineDefault(int fixedX, string text)
        {
            int currentY = Console.CursorTop;
            Console.SetCursorPosition(fixedX, currentY);
            Console.WriteLine(text);
        }
        public static string CustomReadLine(int prompt)
        {
            int currentY = Console.CursorTop;
            Console.SetCursorPosition(49 + prompt + 1, currentY - 1);
            string input = Console.ReadLine();
            Console.SetCursorPosition(49, currentY);
            return input;
        }
        int lol = 1;
        public static void ASCIIArt()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            MenuUtility.CustomWriteLineDefault(49, "⠀⢀⣠⣄⣀⣀⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣤⣴⣶⡾⠿⠿⠿⠿⢷⣶⣦⣤⣀⡀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⢰⣿⡟⠛⠛⠛⠻⠿⠿⢿⣶⣶⣦⣤⣤⣀⣀⡀⣀⣴⣾⡿⠟⠋⠉⠀⠀⠀⠀⠀⠀⠀⠀⠉⠙⠻⢿⣷⣦⣀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣀⣀⣀⣀⣀⣀⣀⡀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠻⣿⣦⡀⠀⠉⠓⠶⢦⣄⣀⠉⠉⠛⠛⠻⠿⠟⠋⠁⠀⠀⠀⣤⡀⠀⠀⢠⠀⠀⠀⣠⠀⠀⠀⠀⠈⠙⠻⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠟⠛⠛⢻⣿");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠈⠻⣿⣦⠀⠀⠀⠀⠈⠙⠻⢷⣶⣤⡀⠀⠀⠀⠀⢀⣀⡀⠀⠙⢷⡀⠸⡇⠀⣰⠇⠀⢀⣀⣀⠀⠀⠀⠀⠀⠀⣀⣠⣤⣤⣶⡶⠶⠶⠒⠂⠀⠀⣠⣾⠟");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠈⢿⣷⡀⠀⠀⠀⠀⠀⠀⠈⢻⣿⡄⣠⣴⣿⣯⣭⣽⣷⣆⠀⠁⠀⠀⠀⠀⢠⣾⣿⣿⣿⣿⣦⡀⠀⣠⣾⠟⠋⠁⠀⠀⠀⠀⠀⠀⠀⣠⣾⡟⠁⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠈⢻⣷⣄⠀⠀⠀⠀⠀⠀⠀⣿⡗⢻⣿⣧⣽⣿⣿⣿⣧⠀⠀⣀⣀⠀⢠⣿⣧⣼⣿⣿⣿⣿⠗⠰⣿⠃⠀⠀⠀⠀⠀⠀⠀⠀⣠⣾⡿⠋⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠙⢿⣶⣄⡀⠀⠀⠀⠀⠸⠃⠈⠻⣿⣿⣿⣿⣿⡿⠃⠾⣥⡬⠗⠸⣿⣿⣿⣿⣿⡿⠛⠀⢀⡟⠀⠀⠀⠀⠀⠀⣀⣠⣾⡿⠋⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠉⠛⠿⣷⣶⣤⣤⣄⣰⣄⠀⠀⠉⠉⠉⠁⠀⢀⣀⣠⣄⣀⡀⠀⠉⠉⠉⠀⠀⢀⣠⣾⣥⣤⣤⣤⣶⣶⡿⠿⠛⠉⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⠉⢻⣿⠛⢿⣷⣦⣤⣴⣶⣶⣦⣤⣤⣤⣤⣬⣥⡴⠶⠾⠿⠿⠿⠿⠛⢛⣿⣿⣿⣯⡉⠁⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠈⣿⣧⡀⠈⠉⠀⠈⠁⣾⠛⠉⠉⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣀⣴⣿⠟⠉⣹⣿⣇⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣸⣿⣿⣦⣀⠀⠀⠀⢻⡀⠀⠀⠀⠀⠀⠀⠀⢀⣠⣤⣶⣿⠋⣿⠛⠃⠀⣈⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⣿⡿⢿⡀⠈⢹⡿⠶⣶⣼⡇⠀⢀⣀⣀⣤⣴⣾⠟⠋⣡⣿⡟⠀⢻⣶⠶⣿⣿⠛⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⣿⣷⡈⢿⣦⣸⠇⢀⡿⠿⠿⡿⠿⠿⣿⠛⠋⠁⠀⣴⠟⣿⣧⡀⠈⢁⣰⣿⠏⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⢻⣦⣈⣽⣀⣾⠃⠀⢸⡇⠀⢸⡇⠀⢀⣠⡾⠋⢰⣿⣿⣿⣿⡿⠟⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠿⢿⣿⣿⡟⠛⠃⠀⠀⣾⠀⠀⢸⡇⠐⠿⠋⠀⠀⣿⢻⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⠁⢀⡴⠋⠀⣿⠀⠀⢸⠇⠀⠀⠀⠀⠀⠁⢸⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢀⣿⡿⠟⠋⠀⠀⠀⣿⠀⠀⣸⠀⠀⠀⠀⠀⠀⠀⢸⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⢸⣿⣁⣀⠀⠀⠀⠀⣿⡀⠀⣿⠀⠀⠀⠀⠀⠀⢀⣈⣿⣿⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            MenuUtility.CustomWriteLineDefault(49, "⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠘⠛⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠿⠟⠛⠋⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀⠀");
            
        }
    }
}
