using System.Runtime.ConstrainedExecution;

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

        static decimal[][] accountsBalance =            
        {
            new decimal [] {10000.0m}, //user1
            new decimal [] {2300.55m, 4500.45m}, //user2
            new decimal [] {10000.0m, 15000.0m, 30000.0m}, //user3
            new decimal [] {1900.0m}, // user4
            new decimal [] {7000.0m, 15000.0m}, //user5
        };

        //start av programmet
        static void Main(string[] args) //ser till att det anges rätt inlogning av användaren
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
                    Console.WriteLine("För många försök, programmet startas om");
                    return;
                }
            }


        }

        //inloggning 
        static bool Login(string[] usernames, string[] passwords, out int userIn)
        {
            userIn = -1; // ogiltig användare 
            int attempts = 0;

            // kollar antal försök som användaren har på sig
            while (attempts < 3)
            {
                Console.Write("Ange ditt användarnamn: ");
                string username = Console.ReadLine();
                Console.Write("Ange ditt Pin-kod: ");
                string password = Console.ReadLine();

                for (int i = 0; i < usernames.Length; i++) //kollar så det är rätt inloggning till varje person
                {
                    if (usernames[i] == username && passwords[i] == password)
                    {
                        userIn = i;
                        return true; // rätt inloggning
                    }
                }
                Console.WriteLine("Fel Användarnamn eller Pin-kod, försök igen!");
                attempts++;
            }
            return false; // ej rätt inloggning efter 3 försök starta om programmet
        }

        // Navigera som användare
        static void Menu(int userIn, string[][] accountsName, decimal[][] accountsBalance)
        {
            while (true) // loopar tills användaren gett rätt siffra 
            {   // menyn om vad användaren kan göra i bankomaten!
                Console.WriteLine("Vad vill du göra?\n" +
                                  "1. Se dina konton och saldon\n" +
                                  "2. Överföring mellan konton\n" +
                                  "3. Ta ut pengar\n" +
                                  "4. Logga ut");

                if (int.TryParse(Console.ReadLine(), out int userChoice)) // ser till att användaren anger en siffra
                {
                    switch (userChoice) // hoppar till methoden användaren valt 
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
                Console.ReadLine(); // bekräftelse att användaren vill återvända till huvudmenyn
            }
        }
        //se konton och saldo
        static void Accounts(int userIn, string[][] accountsName, decimal[][] accountsBalance) // ger oss en bild av användarens konton och saldon
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
            int totalAccounts = accountsBalance[userIn].Length; // kollar hur många konton det finns.
            if (totalAccounts < 2) //om användaren har mindre än 2 konton är överföring ej tillgänglig.
            {
                Console.WriteLine("överföring är inte möjligt pågrund av för lite konton! ");
            }
            else 
            {
                int transferFromAccount = -1;       // håller koll på användarens inmatning om den är rätt eller fel.
                int transferToAccount = -1;
                decimal amountToTransfer = 0;
                
                do     // ser till att fråga om en fråga tills rätt svar anges
                {
                    Console.WriteLine("Välj vilket konto du vill överföra från: ");
                    for (int i = 0; i < totalAccounts; i++)
                    {
                        Console.WriteLine($"{i + 1}. {accountsName[userIn][i]}: {accountsBalance[userIn][i]:C}");
                    }

                    //läsa användarens inmatning av konto den ska överföra från
                    string transferFrom = Console.ReadLine(); 
                    if (int.TryParse(transferFrom, out transferFromAccount) && transferFromAccount > 0 && transferFromAccount <= totalAccounts) // ser till att användaren ger rätt konto att överföra från
                    {
                        transferFromAccount--; // väljer kontot som användaren ger men tar minus 1 då programmet räknar från 0
                    }
                    else
                    {
                        Console.WriteLine("Kontot finns inte, vänligen ange ett konto som finns");
                        transferFromAccount = -1; // ej rätt siffra skriven av användaren
                    }
                } while (transferFromAccount == -1); 

                do // ungefär samma som ovan bara att användaren väljer till vilket konto som ska överföras
                {

                    Console.WriteLine("Välj vilket konto du vill överföra till: ");
                    for (int i = 0; i < totalAccounts; i++)
                    {
                        Console.WriteLine($"{i + 1}. {accountsName[userIn][i]}: {accountsBalance[userIn][i]:C}");
                    }
                    //läsa användarens inmatning av konto den ska överföra till
                    string transferTo = Console.ReadLine();
                    if (int.TryParse(transferTo, out transferToAccount) && transferToAccount > 0 && transferToAccount <= totalAccounts)
                    {
                        transferToAccount--; // väljer kontot som användaren ger men tar minus 1 då programmet räknar från 0
                        if (transferFromAccount == transferToAccount)
                        {
                            Console.WriteLine("Det går tyvärr inte att flytta pengar från och till samma konto!");
                            transferToAccount = -1;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Kontot finns inte, vänligen ange ett konto som finns");
                        transferToAccount = -1;
                    }

                } while (transferToAccount == -1);
                
                bool rightamount = false;  
                do
                {
                    Console.WriteLine("Hur mycket vill du överföra? ");
                    string totalAmount = Console.ReadLine();  // har decimal för att få rätt överföring.
                    if (decimal.TryParse(totalAmount, out  amountToTransfer))
                    {
                        if (amountToTransfer > 0 && amountToTransfer <= accountsBalance[userIn][transferFromAccount] && transferFromAccount != transferToAccount) // är tillför att se till att användaren ger rätt mängd av pengar
                        {
                            rightamount = true;
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
                } while (!rightamount);
                
                accountsBalance[userIn][transferFromAccount] -= amountToTransfer; // tar bort pengar från kontot användaren bett om 
                accountsBalance[userIn][transferToAccount] += amountToTransfer;   // lägger till pengar till kontot användaren bett om
                Console.WriteLine($"\nÖverföring från {accountsName[userIn][transferFromAccount]}t till {accountsName[userIn][transferToAccount]}t lyckades"); 
                Accounts(userIn, accountsName, accountsBalance); // visar användarens konton och saldo

            }
        }
        // ta ut pengar
        static void withdraw(int userIn, string[][] accountsName, decimal[][] accountsBalance)
        {
            int totalAccounts = accountsBalance[userIn].Length;
            int withdrawFromAccount = -1;
            bool rightChoice = false;
            while (!rightChoice)    // loop till för att få rätt konto från användaren
            {
                Console.WriteLine("\nVälj vilket konto du vill ta ut pengar från: ");
                for (int i = 0; i < accountsBalance[userIn].Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {accountsName[userIn][i]}: {accountsBalance[userIn][i]:C}");
                }

                string withdrawAccountChoice = Console.ReadLine(); 
                if (int.TryParse(withdrawAccountChoice, out withdrawFromAccount) && withdrawFromAccount > 0 && withdrawFromAccount <= totalAccounts) // ser till att användaren ger rätt konto
                {
                    withdrawFromAccount--; // väljer kontot som användaren ger men tar minus 1 då programmet räknar från 0
                    rightChoice = true; // om rätt konto är angiven av användaren hoppa till nästa fråga
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
                    accountsBalance[userIn][withdrawFromAccount] -= amountToWithdraw; // tar ut pengar från kontot användaren gett
                    Console.WriteLine($"\nuttag från {accountsName[userIn][withdrawFromAccount]}t gick igenom");
                    Console.WriteLine($"\nDitt saldo efter uttag: {accountsBalance[userIn][withdrawFromAccount]}"); //visar användarens konton och saldo
                }
                else
                {
                    Console.WriteLine("Fel PIN-kod, uttag mysslyckad! ");
                }
            }
        }
    }
}
