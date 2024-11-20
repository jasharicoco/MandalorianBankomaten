namespace MandalorianBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("== Välkommen till Mandalorian Bankomaten ==\n");
            
            Bank bank = new Bank();
            bank.Run();
        }
    }
}