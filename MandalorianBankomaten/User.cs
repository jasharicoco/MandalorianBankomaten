using System.Globalization;

namespace MandalorianBankomaten
{
    public class User
    {
        private List<Account> _accounts;

        public string Name { get; private set; }
        public string Password { get; private set; }
        public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();
        static int userCounter = 0;
        public int userID { get; set; }

        public User(string name, string password)
        {
            userCounter++;
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Namn får inte vara tomt.", nameof(name));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Lösenord får inte vara tomt.", nameof(password));

            Name = name;
            Password = password;
            _accounts = new List<Account>();
            userID = userCounter;
        }

        public void ShowAccounts()
        {
            if (_accounts.Count == 0)
            {
                Console.WriteLine($"Användare: {Name} har inga konton.");
                return;
            }

            Console.WriteLine($"Konton för användare: {Name}");
            foreach (var account in _accounts)
            {
                Console.WriteLine($" - Konto: {account.AccountName}, Saldo: {account.Balance.ToString("C", CultureInfo.CurrentCulture)}");
            }
        }

        public void AddAccount(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account), "Kontot kan inte vara null.");

            _accounts.Add(account);
            Console.WriteLine($"Konto {account.AccountName} har lagts till för användare: {Name}.");
        }

        public void RemoveAccount(Account account)
        {
            if (_accounts.Remove(account))
            {
                Console.WriteLine($"Konto {account.AccountName} har tagits bort för användare: {Name}.");
            }
            else
            {
                Console.WriteLine("Kontot finns inte i listan.");
            }
        }

        public void TransferMoneyBetweenAccounts(Account fromAccount, Account toAccount, decimal amount)
        {
            if (fromAccount == null || toAccount == null)
            {
                Console.WriteLine("Ett eller båda konton är ogiltiga.");
                return;
            }

            if (fromAccount == toAccount)
            {
                Console.WriteLine("Du kan ej överföra från/till samma konto.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Beloppet måste vara större än noll.");
                return;
            }

            if (fromAccount.Balance >= amount)
            {
                fromAccount.Withdraw(amount);
                toAccount.Deposit(amount);
                Console.WriteLine($"Överförde {amount.ToString("C", CultureInfo.CurrentCulture)} från {fromAccount.AccountName} till {toAccount.AccountName}.");
            }
            else
            {
                Console.WriteLine("Otillräckligt saldo för överföring.");
            }
        }

        public void TransferMoneyToUser(User recipient, Account fromAccount, Account recipientAccount, decimal amount)
        {
            if (recipient == null || fromAccount == null || recipientAccount == null)
            {
                Console.WriteLine("Mottagare eller något av kontona är ogiltiga.");
                return;
            }

            if (amount <= 0)
            {
                Console.WriteLine("Beloppet måste vara större än noll.");
                return;
            }

            if (fromAccount.Balance >= amount)
            {
                fromAccount.Withdraw(amount);
                recipientAccount.Deposit(amount);
                Console.WriteLine($"Överförde {amount.ToString("C", CultureInfo.CurrentCulture)} från {fromAccount.AccountName} till {recipient.Name}'s {recipientAccount.AccountName}.");
            }
            else
            {
                Console.WriteLine("Otillräckligt saldo för överföring.");
            }
        }
    }
}