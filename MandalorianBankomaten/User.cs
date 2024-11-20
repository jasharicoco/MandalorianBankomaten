using System.Globalization;

namespace MandalorianBankomaten
{
    public class User
    {
        private List<Account> _accounts;
        private List<Loan> _loans = new List<Loan>(); // Added loans list

        public string Name { get; private set; }
        public string Password { get; private set; }
        public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();
        static int _userCounter = 0;
        public int UserId { get; set; }

        public User(string name, string password)
        {
            _userCounter++;
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Namn får inte vara tomt.", nameof(name));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Lösenord får inte vara tomt.", nameof(password));

            Name = name;
            Password = password;
            _accounts = new List<Account>();
            UserId = _userCounter;
        }
        // Added method to show the user accounts
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
        // Added method to take a loan
        public void TakeLoan(decimal amount, decimal interestRate)
        {
            if (amount <= 0) throw new ArgumentException("Lånebeloppet måste vara större än noll.");
            if (interestRate <= 0) throw new ArgumentException("Räntesatsen måste vara större än noll.");

            Loan loan = new Loan(amount, interestRate);
            _loans.Add(loan);
            Console.WriteLine($"Lån på {amount.ToString("C", CultureInfo.CurrentCulture)} har tagits av användare: {Name}.");
        }
        // Method to display users loans
        public void ShowLoans()
        {
            if (_loans.Count == 0)
            {
                Console.WriteLine($"Användare: {Name} har inga lån.");
                return;
            }

            Console.WriteLine($"Lån för användare: {Name}");
            foreach (var loan in _loans)
            {
                Console.WriteLine($" - Lån: {loan.Amount.ToString("C", CultureInfo.CurrentCulture)}, Ränta: {loan.InterestRate}%");
            }
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

        public async Task TransferMoneyBetweenAccounts(Account fromAccount, Account toAccount, decimal amount)
        {
            if (fromAccount.Balance >= amount)
            {
                Console.WriteLine("Förbereder överföring... Vänta 1 minut.");

                // Vänta i 15 minuter
                await Task.Delay(TimeSpan.FromMinutes(1));

                fromAccount.Withdraw(amount);
                toAccount.Deposit(amount);
                Console.WriteLine($"Överförde {amount.ToString("C", CultureInfo.CurrentCulture)} från {fromAccount.AccountName} till {toAccount.AccountName}.");
            }
            else
            {
                Console.WriteLine("Otillräckligt saldo för överföring.");
            }
        }

        public async Task TransferMoneyToUser(User recipient, Account fromAccount, Account recipientAccount, decimal amount)
        {
            if (fromAccount.Balance >= amount)
            {
                Console.WriteLine("Förbereder överföring... Vänta 1 minut.");

                // Vänta i 15 minuter
                await Task.Delay(TimeSpan.FromMinutes(1));

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