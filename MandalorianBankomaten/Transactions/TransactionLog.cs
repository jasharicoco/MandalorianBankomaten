using MandalorianBankomaten.Menu;

namespace MandalorianBankomaten.Transactions
{
    internal class TransactionLog
    {
        // Private fields
        private readonly string _logFilePath;

        // Constructor
        public TransactionLog(string logFilePath)
        {
            // name for the file with all transactions
            _logFilePath = logFilePath;

            if (!File.Exists(_logFilePath))
            {
                // Creates new file if it does not exist
                File.Create(_logFilePath).Close();
            }
        }

        // Methods
        public void LogTransaction(int userId, string name, string transactionInfo)
        {
            try
            {
                // Variable with log information
                string logText = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} Användare {userId} - {name}: {transactionInfo}";
                // Opens and writes to file then closes it
                File.AppendAllText(_logFilePath, logText + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // error message if try does not work
                MenuUtility.CustomWriteLine(49, $"Loggning misslyckades: {ex.Message}");
            }
        }
        public void ShowTransactionLogs(int userId, string name)
        {
            if (File.Exists(_logFilePath))
            {
                Console.WriteLine($"Transaktionshistorik för användare {name}:");
                // Reads all file-lines and save it to string array
                string[] logs = File.ReadAllLines(_logFilePath);

                foreach (var log in logs.Where(log => log.Contains($"Användare {userId} - {name}:")))
                {
                    Console.WriteLine(log);
                }
            }
            else
            {
                MenuUtility.CustomWriteLine(49, "Transaktionshistoriken är tom.");
            }
        }

    }
}
