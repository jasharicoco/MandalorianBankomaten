using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Channels;

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

        public void CreateAccount()
        {
            Console.WriteLine("Ange namn på konto: ");
            string accountName = Console.ReadLine();
            Console.WriteLine("Ange valuta: ");
            string currency = Console.ReadLine();
            Console.WriteLine("Ange insättningsbelopp: ");
            decimal deposit = Convert.ToDecimal(Console.ReadLine());

            Account account = new Account(accountName, deposit, currency);
            if (account == null) throw new ArgumentNullException(nameof(account), "Kontot kan inte vara null.");

            _accounts.Add(account);
            Console.WriteLine($"Konto {account.AccountName} har lagts till för användare: {Name}.");
        }
       
        public void AddAccount(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account), "Kontot kan inte vara null.");

            _accounts.Add(account);
            Console.WriteLine($"Konto {account.AccountName} har lagts till för användare: {Name}.");
        }
        // Added method to take a loan
        public void TakeLoan(decimal interestRate)
        {
            decimal sum = 0;
            decimal amount = 0;
            foreach (Account account in _accounts)
            {
                sum =+ account.Balance;
            }
            do
            {
                // Since the user will be prompted to input values I think we can remove the method arguments
                Console.WriteLine("Ange lånebelopp");
                amount = Convert.ToDecimal(Console.ReadLine());
                if (amount > sum*5)
                {
                    Console.WriteLine("Vänligen ange ett lägre belopp (Lånebelopp har en gräns på 5 gånger ditt banksaldo");
                }
                if (amount <= 0) throw new ArgumentException("Lånebeloppet måste vara större än noll.");
            } while (amount > sum * 5);

            if (interestRate <= 0) throw new ArgumentException("Räntesatsen måste vara större än noll.");

            Loan loan = new Loan(amount, interestRate);
            _loans.Add(loan);
            Console.WriteLine($"Lån på {amount.ToString("C", CultureInfo.CurrentCulture)} har tagits av användare: {Name}.");
            Console.WriteLine($"Räntekostnad: {loan.MonthlyInterest().ToString("C", CultureInfo.CurrentCulture)} i månaden.");
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
                Console.WriteLine($" - Lån: {loan.Amount.ToString("C", CultureInfo.CurrentCulture)}, Räntesats: {loan.InterestRate}%, Räntekostnad: {loan.MonthlyInterest().ToString("C", CultureInfo.CurrentCulture)} i månaden.");

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