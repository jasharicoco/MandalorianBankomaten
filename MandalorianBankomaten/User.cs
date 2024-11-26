using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Channels;

namespace MandalorianBankomaten
{
    public class User : IUser // User class implements IUser interface
    {
        private List<Account> _accounts;
        private List<Loan> _loans;

        public string Name { get; private set; }
        public string Password { get; private set; }
        public int UserId { get; private set; }
        public List<Account> Accounts => _accounts;
        static int _userCounter = 0;
        
        public User(string name, string password)
        {
            _userCounter++;
            Name = name;
            Password = password;
            _accounts = new List<Account>();
            _loans = new List<Loan>();
            UserId = _userCounter;
        }

        // Show number of accounts
        public int NumberOfAccounts()
        {
            return _accounts.Count;
        }

        // See if user has accounts
        public bool HasAccounts()
        {
            if (_accounts.Count == 0)
            {
                Console.WriteLine($"{Name} har inga konton på den här banken.");
                return false;
            }
            return true;
        }

        // Method to show accounts
        public void ShowAccounts()
        {
            Console.WriteLine($"Konton för användare: {Name}");
            for (int i = 0; i < _accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_accounts[i].AccountName} - Saldo: {_accounts[i].Balance:C}");
            }
        }

        public void CreateAccount()
        {
            Console.WriteLine("Ange namn på konto:");
            string accountName = Console.ReadLine();
            Console.WriteLine("Ange konto-valuta:");
            string currencyCode = Console.ReadLine();
            Console.WriteLine("Ange insättningsbelopp:");
            decimal depositAmount = Convert.ToDecimal(Console.ReadLine());

            Account newAccount = new Account(accountName, depositAmount, currencyCode);
            _accounts.Add(newAccount);
            Console.WriteLine($"Konto {accountName} har skapats!");
            Console.ReadKey();
        }

        public void RemoveAccount(Account account)
        {
            _accounts.Remove(account);
        }

        public async Task TransferMoneyBetweenAccounts(Account fromAccount, Account toAccount, decimal amount)
        {
            if (fromAccount.Balance >= amount)
            {
                Console.WriteLine("Förbereder överföring... Vänta 1 minut.");
                await Task.Delay(TimeSpan.FromMinutes(1));

                fromAccount.Withdraw(amount);
                toAccount.Deposit(amount);
                Console.WriteLine($"Överföring från {fromAccount.AccountName} till {toAccount.AccountName} lyckades.");
            }
            else
            {
                Console.WriteLine("Otillräckligt saldo för överföring.");
            }
        }

        public async Task TransferMoneyToUser(User recipient, Account fromAccount, Account recipientAccount,
            decimal amount)
        {
            if (fromAccount.Balance >= amount)
            {
                Console.WriteLine("Förbereder överföring... Vänta 1 minut.");
                await Task.Delay(TimeSpan.FromMinutes(1));

                fromAccount.Withdraw(amount);
                recipientAccount.Deposit(amount);
                Console.WriteLine(
                    $"Överföring från {fromAccount.AccountName} till {recipient.Name}'s {recipientAccount.AccountName} lyckades.");
            }
            else
            {
                Console.WriteLine("Otillräckligt saldo för överföring.");
            }
        }
        
        // Method to take a loan
        public void TakeLoan(decimal amount, decimal interestRate)
        {
            Loan newLoan = new Loan(amount, interestRate);
            _loans.Add(newLoan); 
            Console.WriteLine($"Lån på {amount} SEK har tagits med ränta på {interestRate}%.");
        }

        // Method to show loans
        public void ShowLoans()
        {
            if (_loans.Count == 0)
            {
                Console.WriteLine("Inga lån registrerade.");
                return;
            }

            foreach (var loan in _loans) 
            {
                Console.WriteLine($"Lån: {loan.Amount} SEK, Ränta: {loan.InterestRate}%, Saldo: {loan.RemainingBalance} SEK");
            }
        }
    }
    
}