namespace MandalorianBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var user = new User("Mandalorian", "1234");
            
            user.Accounts.Add(new Account("Lönekonto", 10000, "SEK"));
            user.Accounts.Add(new Account("Sparkonto", 50000, "SEK"));

            // Visa användarens konton och saldon
            user.ShowAccounts();

            // Skapa en annan användare utan konton
            User user2 = new User("Yoda", "1111");

            // Visa hans konton
            user2.ShowAccounts();
            
        }
    }
}
