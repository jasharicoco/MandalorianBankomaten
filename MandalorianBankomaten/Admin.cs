using System.Reflection.Metadata;

namespace MandalorianBankomaten
{
    public class Admin
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public Admin(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public List<User> CreateUser(List<User> users)
        {
            Console.Write("Vänligen skriv in ett användarnamn");
            string newusername = Console.ReadLine().ToLower();

            bool usernameExists = users.Any(user => user.Name.ToLower() == newusername);

            if (usernameExists)
            {
                Console.WriteLine("Användarnamnet är redan upptaget. Försök med ett annat.");
                return users;
            }
            else
            {
                Console.Write("Vänligen skriv in ett lösenord");
                string newpswd = Console.ReadLine();
                User newUser = new User(newusername, newpswd);
                users.Add(newUser);
                Console.WriteLine("Användaren har skapats!");
                return users;
            }
        }
    }
}
