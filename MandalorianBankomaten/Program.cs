namespace MandalorianBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("== Välkommen till Mandalorian Bankomaten ==\n");

            // Skapa användare
            var user1 = new User("Mandalorian", "1234");
            Console.WriteLine($"Användare skapad: {user1.Name}\n");

            // Skapa och lägga till konton till användaren
            var account1 = new Account("Lönekonto", 10000, "SEK");
            var account2 = new Account("Sparkonto", 50000, "SEK");
            user1.AddAccount(account1);
            user1.AddAccount(account2);

            // Visa användarens konton och saldon
            Console.WriteLine("\nVisar konton för Mandalorian:");
            user1.ShowAccounts();

            // Skapa en annan användare utan konton
            var user2 = new User("Yoda", "1111");
            Console.WriteLine($"\nAnvändare skapad: {user2.Name}");

            // Visa konton för den nya användaren
            Console.WriteLine("\nVisar konton för Yoda:");
            user2.ShowAccounts();

            // Skapa och lägga till konto till Yoda
            var account3 = new Account("Lönekonto", 5000, "USD");
            user2.AddAccount(account3);

            Console.WriteLine("\nEfter att ha lagt till ett konto till Yoda:");
            user2.ShowAccounts();

            // Avsluta programmet
            Console.WriteLine("\n== Tack för att du använde Mandalorian Bankomaten! ==");
        }
    }
}