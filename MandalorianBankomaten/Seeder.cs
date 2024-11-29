namespace MandalorianBankomaten
{
    internal class Seeder
    {
        // Creating seed users
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

        // Adding seed accounts to each user
        public static void AddStandardAccountsToUsers(List<User> users)
        {
            foreach (var user in users)
            {
                // Create standard accounts for each user
                user.AddAccount(new Account("Lönekonto", 10000, "SEK"));
                user.AddAccount(new Account("Sparkonto", 50000, "SEK"));
            }
        }
    }
}
