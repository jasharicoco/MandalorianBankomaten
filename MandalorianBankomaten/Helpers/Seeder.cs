using MandalorianBankomaten.Accounts;
using MandalorianBankomaten.Users;

namespace MandalorianBankomaten.Helpers
{
    internal class Seeder
    {
        // Creating seed customer
        public static List<User> SeedUsers()
        {
            return new List<User>
            {
                new User("viggo", "1234"),
                new User("egzon", "1234"),
                new User("alex", "1234"),
                new User("margarita", "1234"),
                new User("tim", "1234"),
                new User("arbunit", "1234"),
            };
        }

        // Creating seed admin user
        public static List<Admin> SeedAdmins()
        {
            return new List<Admin>
            {
                new Admin("admin", "0000")
            };
        }

        // Creating and adding seed accounts to seed users
        public static void AddSeedAccountsToUsers(List<User> users)
        {
            foreach (var user in users)
            {
                // Create standard accounts for each user
                user.AddAccount(new Account("Lönekonto", "SEK", 10000));
                user.AddAccount(new Account("Sparkonto", "SEK", 50000));
            }
        }

    }
}