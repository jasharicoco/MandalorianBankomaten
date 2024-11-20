namespace MandalorianBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Skapa användare
            var user1 = new User("Mandalorian", "1234");
            
            // Skapa konto
            user1.Accounts.Add(new Account("Lönekonto", 10000, "SEK"));
            user1.Accounts.Add(new Account("Sparkonto", 50000, "SEK"));

            // Visa användarens konton och saldon
            user1.ShowAccounts();

            // Skapa en annan användare utan konton
            User user2 = new User("Yoda", "1111");

            // Visa hans konton
            user2.ShowAccounts();

            // Skapa konto till user2
            user2.Accounts.Add(new Account("Lönekonto", 5000, "USD"));
        }
    }
}
