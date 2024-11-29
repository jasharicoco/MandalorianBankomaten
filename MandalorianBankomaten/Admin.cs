namespace MandalorianBankomaten
{
    public class Admin
    {
        public string Name { get; private set; }
        public string Password { get; private set; }
        public int UserId { get; private set; }
        public List<Account> Accounts { get; private set; } = new List<Account>();

        static int _adminCounter = 0;

        public Admin(string name, string password)
        {
            _adminCounter++;
            Name = name;
            Password = password;
            UserId = _adminCounter;
        }

        public List<User> CreateUser(List<User> users)
        {
            Console.Write("Vänligen skriv in ett användarnamn: ");
            string newUsername = Console.ReadLine().ToLower();

            bool usernameExists = users.Any(user => user.Name.ToLower() == newUsername);

            if (usernameExists)
            {
                Console.WriteLine("Användarnamnet är redan upptaget. Försök med ett annat.");
                return users;
            }

            Console.Write("Vänligen skriv in ett lösenord: ");
            string newPassword = Console.ReadLine();

            User newUser = new User(newUsername, newPassword);
            users.Add(newUser);
            Console.WriteLine("Användaren har skapats!");
            return users;
        }

        public List<User> DeleteUser(List<User> users)
        {
            if (users.Count == 0)
            {
                Console.WriteLine("Det finns inga användare att ta bort.");
                return users; // If there are no users, return the list as it is
            }

            Console.WriteLine("Lista över alla användare:");
            foreach (var user in users)
            {
                Console.WriteLine($"👽 {user.Name}");
            }

            Console.Write("Ange namnet på den användare du vill ta bort: ");
            string usernameToRemove = Console.ReadLine();

            // Use helper function to check if the user exists
            var userToRemove = FindUserByName(users, usernameToRemove);

            if (userToRemove != null)
            {
                Console.WriteLine($"Är du säker på att du vill ta bort användaren '{userToRemove.Name}'? (j/n)");
                string answer = Console.ReadLine().ToLower();

                if (answer == "j")
                {
                    users.Remove(userToRemove);
                    Console.WriteLine($"Användare '{userToRemove.Name}' har tagits bort.");
                }
                else
                {
                    Console.WriteLine("Åtgärden avbröts. Ingen användare har tagits bort.");
                }
            }
            else
            {
                Console.WriteLine($"Ingen användare med namnet '{usernameToRemove}' hittades.");
            }

            Console.ReadKey();
            return users; // Return the updated list of users
        }

        // Helper function to find a user by name
        private User FindUserByName(List<User> users, string username)
        {
            return users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
