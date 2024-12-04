using MandalorianBankomaten.Users;

namespace MandalorianBankomaten.Accounts
{
    internal class Admin
    {
        // Private fields
        private string _name;
        private string _password;
        private int _userId;
        private List<Account> _accounts = new();

        // Static counter for generating unique admin IDs
        private static int _adminCounter = 0;

        // Public properties
        public string Name
        {
            get => _name;
            private set
            {
                //if (string.IsNullOrWhiteSpace(value))
                //throw new ArgumentException("Name cannot be null or empty.");
                _name = value;
            }
        }
        public string Password
        {
            get => _password;
            private set
            {
                //if (string.IsNullOrWhiteSpace(value))
                //throw new ArgumentException("Password cannot be null or empty.");
                _password = value;
            }
        }
        public int UserId => _userId; // Read-only; auto-assigned by constructor
        public List<Account> Accounts => new List<Account>(_accounts); // Returns a copy to protect the internal list

        // Constructor
        public Admin(string name, string password)
        {
            _adminCounter++;
            _userId = _adminCounter;
            Name = name;
            Password = password;
        }

        // Methods
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
        private User FindUserByName(List<User> users, string username)
        {
            return users.FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
        public override string ToString()
        {
            return $"Name: {Name}, UserID: {UserId}";
        }

    }
}
