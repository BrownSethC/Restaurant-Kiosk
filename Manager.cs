using System;
using System.IO;
using System.Threading;
using static System.Console;

class Manager
{
    public const string day = @"C:\Users\Kiosk Records\Daily Records\Daily Kiosk Records.txt";
    public const string week = @"C:\Users\Kiosk Records\Week Records\Weekly Kiosk Records.txt";
    public const string allTime = @"C:\Users\Kiosk Records\All Time Records\All Kiosk Records.txt";

    public static decimal TaxRate = decimal.Parse(File.ReadAllText(@"C:\Users\Kiosk Records\Menus and Prices\Tax Rate.txt"));

    public static void ChangeTax(decimal newRate)
    {
        TaxRate = newRate;
        File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Tax Rate.txt", TaxRate.ToString());
    }

    public static void ChangeTaxRate()
    {
        Write("Enter the new tax rate: ");
        string tryRate = ReadLine();
        decimal.TryParse(tryRate, out decimal newTaxRate);
        if (newTaxRate > 0.25M)
        {
            WriteLine($"The rate you entered is {newTaxRate:P}.");
            Write("That is a high rate. To confirm this rate, enter 1: ");
            tryRate = ReadLine();
            if (tryRate == "1")
            {
                ChangeTax(newTaxRate);
                WriteLine($"The new tax rate is {TaxRate:P}");
            }
            else
            {
                WriteLine("The tax rate was not changed.");
            }
        }
        else if (newTaxRate == 0 && tryRate == newTaxRate.ToString())
        {
            WriteLine($"The rate you entered is {newTaxRate:P}.");
            Write("There will be no tax. To confirm this rate, enter 1: ");
            tryRate = ReadLine();
            if (tryRate == "1")
            {
                ChangeTax(newTaxRate);
                WriteLine($"The new tax rate is {TaxRate:P}");
            }
            else
            {
                WriteLine("The tax rate was not changed.");
            }
        }
        else if (newTaxRate > 0 && newTaxRate < 0.25M)
        {
            ChangeTax(newTaxRate);
            WriteLine($"The new tax rate is {TaxRate:P}");
        }
        else
        {
            WriteLine("The tax rate was not changed.");
        }
    }

    public static void EditMenu(string menu)
    {
        bool doneEditing = false;
        while (!doneEditing)
        {
        WriteLine("\nEditing menu:");
        WriteLine(" - Enter 1 to add an item to the menu.");
        WriteLine(" - Enter 2 to change an item on the menu.");
        WriteLine(" - Enter 3 to remove an item from the menu.");
        WriteLine(" - Enter anything else to return to the manager menu.");
        string choice = ReadLine();
        if (choice == "1")
        {
            Menu.AddToMenu(menu);
        }
        else if (choice == "2")
        {
            Menu.EditMenuItem(menu);
        }
        else if (choice == "3")
        {
            Menu.RemoveFromMenu(menu);
        }
        else
        {
            doneEditing = true;
        }
        }
    }

    public static void AddSaleToAllRecords(string fileName, decimal saleBeforeTax, decimal tax)
    {
        if (fileName == "Daily")
        {
            AddSaleToRecord(fileName, saleBeforeTax, tax);
            fileName = "Weekly";
        }
        if (fileName == "Weekly")
        {
            AddSaleToRecord(fileName, saleBeforeTax, tax);
            fileName = "All Time";
        }
        if (fileName == "All Time")
        {
            AddSaleToRecord(fileName, saleBeforeTax, tax);
        }
    }

    public static void AddSaleToRecord(string fileName, decimal saleBeforeTax, decimal tax)
    {
        string directory = "";
        if (fileName == "Daily")
        {
            directory = day;
        }
        else if (fileName == "Weekly")
        {
            directory = week;
        }
        else
        {
            directory = allTime;
        }
        
        string text = File.ReadAllText(directory);
        string[] findMoney = text.Split("$");
        decimal salesCurrent = 0M;
        decimal taxCurrent = 0M;

        int period = findMoney[1].IndexOf(".");
        var line = findMoney[1].Substring(0, period + 3);
        decimal.TryParse(line, out decimal money);
        salesCurrent = money;
        
        period = findMoney[2].IndexOf(".");
        line = findMoney[2].Substring(0, period + 3);
        decimal.TryParse(line, out money);
        taxCurrent = money;
        
        decimal salesTotal = salesCurrent + saleBeforeTax;
        decimal taxTotal = taxCurrent + tax;
        text = text.Replace(salesCurrent.ToString(), salesTotal.ToString());
        text = text.Replace(taxCurrent.ToString(), taxTotal.ToString());
        File.WriteAllText(directory, text);
    }
    
    public static void ReadFile(string fileName)
    {
        string directory = "";
        if (fileName == "Daily")
        {
            directory = day;
        }
        else if (fileName == "Weekly")
        {
            directory = week;
        }
        else
        {
            directory = allTime;
        }

        string text = File.ReadAllText(directory);
        WriteLine(text);
    }
    public static void ManagerAccess()
        {
            //string username = "Manager";
            //string password = "Manag3r";
            WriteLine("\nIdentification required to access these files.");
            Write("Username: ");
            string username = ReadLine();
            Write("Please enter password: ");
            string password = ReadLine();
            WriteLine("Checking credentials...");
            Thread.Sleep(2000);
            if (username == "Manager" && password == "Manag3r")
            {
                WriteLine("Identification recognized. Access to files granted.");
                bool exit = false;
                while (!exit)
                {
                    WriteLine("\nWhat would you like to access?");
                    WriteLine(" - Enter 1 to change the tax rate.");
                    WriteLine(" - Enter 2 to edit a menu.");
                    WriteLine(" - Enter 3 to view the daily records.");
                    WriteLine(" - Enter 4 to view the weekly records.");
                    WriteLine(" - Enter 5 to view all records.");
                    WriteLine(" - Enter anything else to return to the ordering menu.");
                    int.TryParse(ReadLine(), out int choice);
                    WriteLine();
                    Thread.Sleep(1000);

                    if (choice == 1)
                    {
                        Manager.ChangeTaxRate();
                    }
                    else if (choice == 2)
                    {
                        WriteLine("\nWhich menu would you like to edit? Options are breakfast, lunch, or dinner.");
                        string menu = ReadLine();
                        menu = menu.ToLower();
                        if (menu == "breakfast" || menu == "lunch" || menu == "dinner")
                        {
                            Manager.EditMenu(menu);
                        }
                        else if (menu == "")
                        {
                            WriteLine("No menu was selected to edit.");
                        }
                        else
                        {
                            WriteLine("That is not an option.");
                        }
                    }
                    else if (choice == 3)
                    {
                        Manager.ReadFile("Daily");
                        Thread.Sleep(1000);
                    }
                    else if (choice == 4)
                    {
                        Manager.ReadFile("Weekly");
                        Thread.Sleep(1000);
                    }
                    else if (choice == 5)
                    {
                        Manager.ReadFile("Daily");
                        Thread.Sleep(500);
                        Manager.ReadFile("Weekly");
                        Thread.Sleep(500);
                        Manager.ReadFile("All Time");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        exit = true;
                    }
                }
            }
            else
            {
                WriteLine("Credentials not recognized. Access denied.");
            }    
        }
}