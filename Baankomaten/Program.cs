namespace Baankomaten
{
    internal class Program
    {
        static string[] usernames = ["user1", "user2", "user3", "user4", "user5"];
        static string[] passwords = ["1234", "2345", "3456", "4567", "5678"];
        static string[][] accountsName =
        {
            new string[] {"Allkonto"}, //konto för user1
            new string [] {"Allkonto", "sparkonto"}, //konto för USer2
            new string [] {"Allkonto", "sparkonto", "sparkonto2"}, //konto för USer3
            new string [] {"Allkonto"}, //konto för USer4
            new string [] {"Allkonto", "sparkonto"}, //konto för USer5
        };

        static decimal[][] accountsBalance =            //decimal ska fixas!!!!!!!!!!!!!!!
        {
            new decimal [] {10000.0m}, //user1
            new decimal [] {2300.0m, 4500.0m}, //user2
            new decimal [] {10000.0m, 15000.0m, 30000.0m}, //user3
            new decimal [] {1900.0m}, // user4
            new decimal [] {7000.0m, 15000.0m}, //user5
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Välkommen till SB Banken!");

            while (true)
            {
                if (Login(usernames, passwords, out int userIn))
                {
                    Menu(userIn, accountsName, accountsBalance);
                }
                else
                {
                    Console.WriteLine("För mångs försök, programmet startas om");
                    break;
                }
            }


        }

        // start och inloggning 
        static bool Login(string[] usernames, string[] passwords, out int userIn)
        {
            userIn = -1; // ogiltig användare 
            int attempts = 0;

            // kollar antal försök
            while (attempts < 3)
            {
                Console.Write("Ange ditt användarnamn: ");
                string username = Console.ReadLine();
                Console.Write("Ange ditt Pin-kod: ");
                string password = Console.ReadLine();

                for (int i = 0; i < usernames.Length; i++)
                {
                    if (usernames[i] == username && passwords[i] == password)
                    {
                        userIn = i;
                        return true; // rätt inloggning
                    }
                }
                attempts++;
                Console.WriteLine("Fel Användarnamn eller Pin-kod, försök igen!");
            }
            return false; // ej rätt inloggning efter 3 försök
        }

        // Navigera som användare
        static void Menu(int userIn, string[][] accountsName, decimal[][] accountsBalance)
        {
            while (true)
            {
                Console.WriteLine("Vad vill du göra?\n" +
                                  "1. Se dina konton och saldon\n" +
                                  "2. Överföring mellan konton\n" +
                                  "3. Ta ut pengar\n" +
                                  "4. Logga ut");

                if (int.TryParse(Console.ReadLine(), out int userChoice))
                {
                    switch (userChoice)
                    {
                        case 1:
                            Accounts(userIn, accountsName, accountsBalance);
                            break;
                        case 2:
                            transfer(userIn, accountsName, accountsBalance);
                            break;
                        case 3:
                            withdraw(userIn, accountsName, accountsBalance);
                            break;
                        case 4:
                            Console.WriteLine("Loggar ut, ha det bra! ");
                            return;
                        default:
                            Console.WriteLine("Var vänlig och välj ett av alternativen ovan!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltigt val, försök igen!");
                }
                Console.WriteLine("Klicka på valfri tangent för att komma till huvudmenyn");
                Console.ReadLine(); // bekräftelse från användaren
            }
        }
        //se konton och saldo
        static void Accounts(int userIn, string[][] accountsName, decimal[][] accountsBalance)
        {
            Console.WriteLine("Dina konton och saldo: ");
            for (int i = 0; i < accountsBalance[userIn].Length; i++)
            {
                Console.WriteLine($"{accountsName[userIn][i]}: {accountsBalance[userIn][i]:C}");
            }
        }

        //överföring mellan konton
        static void transfer(int userIn, string[][] accountsName, decimal[][] accountsBalance)
        {
            int totalAccounts = accountsBalance[userIn].Length;
            if (totalAccounts < 2) //om användaren har mindre än 2 konton är överföring ej tillgänglig.
            {
                Console.WriteLine("överföring är inte möjligt pågrund av för lite konton! ");
            }

            if (totalAccounts >= 2)
            {
                int transferFromAccount = -1;
                int transferToAccount = -1;
                bool rightChoiceOfTransfers = false;
                while (!rightChoiceOfTransfers)
                {
                    Console.WriteLine("Välj vilket konto du vill överföra från: ");
                    for (int i = 0; i < accountsBalance[userIn].Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {accountsName[userIn][i]}: {accountsBalance[userIn][i]:C}");
                    }

                    //läsa användarens inmatning av konto den ska överföra från
                    string transferFrom = Console.ReadLine(); //-1 då arrayen börjar från 0 och inte 1
                    if (int.TryParse(transferFrom, out transferFromAccount) && transferFromAccount > 0 && transferFromAccount <= totalAccounts)
                    {
                        transferFromAccount--; // väljer kontot som användaren ger men tar minus 1 då programmet räknar från 0
                    }
                    else
                    {
                        Console.WriteLine("Kontot finns inte, vänligen ange ett konto som finns");
                        
                    }

                    Console.WriteLine("Välj vilket konto du vill överföra till: ");
                    for (int i = 0; i < accountsBalance[userIn].Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {accountsName[userIn][i]}: {accountsBalance[userIn][i]:C}");
                    }
                    //läsa användarens inmatning av konto den ska överföra till
                    string transferTo = Console.ReadLine();
                    if (int.TryParse(transferTo, out transferToAccount) && transferToAccount > 0 && transferToAccount <= totalAccounts)
                    {
                        transferToAccount--; // väljer kontot som användaren ger men tar minus 1 då programmet räknar från 0
                    }
                    else
                    {
                        Console.WriteLine("Kontot finns inte, vänligen ange ett konto som finns");
                    }

                    if (transferFromAccount == transferToAccount)
                    {
                        Console.WriteLine("Det går tyvärr inte att flytta pengar från och till samma konto!");

                    }
                    rightChoiceOfTransfers = true;
                }
                Console.WriteLine("Hur mycket vill du överföra? ");
                string totalAmount = Console.ReadLine();  // har decimal för att få rätt överföring.
                if (decimal.TryParse(totalAmount, out decimal amountToTransfer))
                {


                    if (amountToTransfer > 0 && amountToTransfer <= accountsBalance[userIn][transferFromAccount] && transferFromAccount != transferToAccount)
                    {
                        accountsBalance[userIn][transferFromAccount] -= amountToTransfer;
                        accountsBalance[userIn][transferToAccount] += amountToTransfer;
                        Console.WriteLine($"Överföring från konto {accountsName[userIn][transferFromAccount]} till {accountsName[userIn][transferToAccount]} lyckades");
                        Accounts(userIn, accountsName, accountsBalance);

                    }
                    else
                    {
                        Console.WriteLine("Summan du angav finns tyvärr inte i kontot du vill överföra från!");
                    }
                }
                else
                {
                    Console.WriteLine("Vänligen Ange ett riktigt belopp");
                }
            }
        }
        // ta ut pengar
        static void withdraw(int userIn, string[][] accountsName, decimal[][] accountsBalance)
        {
            int totalAccounts = accountsBalance[userIn].Length;
            int withdrawFromAccount = -1;
            bool rightChoice = false;
            while (!rightChoice)
            {
                Console.WriteLine("\nVälj vilket konto du vill ta ut pengar från: ");
                for (int i = 0; i < accountsBalance[userIn].Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {accountsName[userIn][i]}: {accountsBalance[userIn][i]:C}");
                }

                string withdrawAccountChoice = Console.ReadLine(); //-1 då arrayen börjar från 0 och inte 1
                if (int.TryParse(withdrawAccountChoice, out withdrawFromAccount) && withdrawFromAccount > 0 && withdrawFromAccount <= totalAccounts)
                {
                    withdrawFromAccount--; // väljer kontot som användaren ger men tar minus 1 då programmet räknar från 0
                    rightChoice = true;
                }
                else
                {
                    Console.WriteLine("kontot finns inte, vänligen ange ett giltigt konto!");
                }
            }
            decimal amountToWithdraw = 0;
            bool rightAmount = false;
            while (!rightAmount)
            {
                Console.WriteLine("\nHur mycket vill du ta ut? ");
                string userInput = Console.ReadLine();  // har decimal för att få rätt överföring.
                if (decimal.TryParse(userInput, out amountToWithdraw))
                {
                    if (amountToWithdraw > 0 && amountToWithdraw <= accountsBalance[userIn][withdrawFromAccount]) //ser till att det är är rätt belopp
                    {
                        rightAmount = true;
                    }
                    else
                    {
                        Console.WriteLine("Du kan tyvärr inte ta ut mer än det som finns på ditt konto!");
                    }
                }
                else
                {
                    Console.WriteLine("ej giltig belopp, vänligen ange ett giltigt belopp");
                }
            }
            bool rightPin = false;
            while (!rightPin)
            {
                // för att kunna göra uttag behövs PIN-kod
                Console.WriteLine("\nPIN-kod krävs för att kunna göra uttag: ");
                string userPin = Console.ReadLine();

                if (userPin == passwords[userIn]) // Om pin-kod är correct görs uttag från det valda kontot.
                {
                    rightPin = true;
                    accountsBalance[userIn][withdrawFromAccount] -= amountToWithdraw;
                    Console.WriteLine($"\nuttag från {accountsName[userIn][withdrawFromAccount]}t gick igenom");
                    Console.WriteLine($"\nDitt saldo efter uttag: {accountsBalance[userIn][withdrawFromAccount]}");
                }
                else
                {
                    Console.WriteLine("Fel PIN-kod, uttag mysslyckad! ");
                }
            }
        }
    }
}
