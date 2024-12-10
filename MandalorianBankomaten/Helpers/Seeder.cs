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
                new User("user1", "password1"),
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
        // The methods picks random currency for the seeded account
        // Possible currencies are listed in list "currencies"
        // Possible account names/types are listed in list "accountTypes"
        // These are also picked by random.
        public static void AddSeedAccountsToUsers(List<User> users)
        {
            var random = new Random();

            // List of possible currencies at Mandalorian
            var currencies = new List<string> { "SEK", "USD", "EUR", "GBP", "DKK", "JPY" };

            // List of possible account types
            var accountTypes = new List<string>
    {
        "Sportkonto",
        "Resekonto",
        "Sparkonto",
        "Lönekonto",
        "Aktiekonto",
        "Fondkonto",
        "Skattekonto",
        "Företagskonto",
        "Gamingkonto",
        "Barnkonto",
        "Möbelkonto",
        "Vattenpolokonto",
        "Cykelkonto"
    };

            foreach (var user in users)
            {
                // Create 1 to 3 accounts per user (picked by random)
                int numberOfAccounts = random.Next(1, 4);

                for (int i = 0; i < numberOfAccounts; i++)
                {
                    // Choose random account type
                    string accountName = accountTypes[random.Next(accountTypes.Count)];

                    // Choose random currency
                    string currency = currencies[random.Next(currencies.Count)];

                    // Generate a random balance in named range
                    decimal balance = currency switch
                    {
                        "JPY" => random.Next(5000, 500000), // JPY: högre belopp eftersom valutan har lägre värde
                        "DKK" => random.Next(1000, 50000),  // DKK: medelstora belopp
                        _ => random.Next(500, 50000)         // Övriga valutor med ungefärligt lika värden
                    };

                    // Assign the account to user
                    user.AddAccount(new Account(accountName, currency, balance));
                }
            }
        }

    }
}