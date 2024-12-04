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
        public void LogTransaction(string transactionInfo)
        {
            try
            {
                // Variable with log information
                string logText = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {transactionInfo}";
                // Opens and writes to file then closes it
                File.AppendAllText(_logFilePath, logText + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // error message if try does not work
                Console.WriteLine($"Loggning misslyckades: {ex.Message}");
            }
        }
        public void ShowTransactionLogs()
        {
            // Name for the file with transactions
            string logFilePath = "TransactionLog.txt";

            if (File.Exists(logFilePath))
            {
                Console.WriteLine("Transaktionshistorik: ");
                // Reads all file-lines and save it to string array
                string[] logs = File.ReadAllLines(logFilePath);

                foreach (var log in logs)
                {
                    Console.WriteLine(log);
                }
            }
            else
            {
                Console.WriteLine("Transaktionshistoriken är tom.");
            }
        }

    }
}
