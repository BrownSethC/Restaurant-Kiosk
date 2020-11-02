using System;
using static System.Console;

namespace Restaurant_Kiosk
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("Welcome to Snowtopia Restaurant!\n\n");
            WriteLine("Which menu would you like to access?");
            WriteLine(" - Enter 1 for the breakfast menu.");
            WriteLine(" - Enter 2 for the lunch menu.");
            WriteLine(" - Enter 3 for the dinner menu.");
            WriteLine(" - Enter anything else to cancel.");
            string menu = RecieveInput();
            if (menu == "1")
            {
                menu = "Breakfast";
            }
            else if (menu == "2")
            {
                menu = "Lunch";
            }
            else if (menu == "3")
            {
                menu = "Dinner";
            }
            else if (menu != "Records checked.")
            {
                menu = "Order cancelled.";
            }

            if (menu == "Order cancelled." || menu == "Order complete.")
            {
                WriteLine("Thanks for coming to Snowtopia! Have a snow-rrific day! This one");
            }
            else if (menu != "Records checked.")
            {
                bool orderDone = false;
                Menu.PrintOutMenu(Menu.FindMenu(menu));
                int[] order = new int[20];
                while (!orderDone)
                {
                    order = AddToOrder(menu, order);
                    WriteLine("\nIs there anything else you would like?");
                    WriteLine("Type 'yes' to continue, 'no' to finish your order, or 'exit' to cancel your order.");
                    string more = RecieveInput();
                    if (more.ToLower() == "y" || more.ToLower() == "yes")
                    {
                        orderDone = false;
                    }
                    else if  (more.ToLower() == "n" || more.ToLower() == "no")
                    {
                        orderDone = true;
                    }
                    else if (more == "Order cancelled.")
                    {
                        orderDone = true;
                        WriteLine("Your order has been cancelled.");
                        WriteLine("Thanks for coming to Snowtopia! Have a snow-rrific day!");
                    }
                    else if (more == "Records checked.")
                    {
                        WriteLine("...");
                        System.Threading.Thread.Sleep(1500);
                        WriteLine("Files locked.");
                    }
                }
                FinishOrder(menu, order);
            }
            else
            {
                WriteLine("...");
                System.Threading.Thread.Sleep(1500);
                WriteLine("Files locked.");
            }
        }
        static int[] FillOrder(string item, string menu, int[] order)
        {
            int location = Menu.FindInMenu(item, menu);
            string[] name = Menu.FindMenuItems(menu);
            if (location == -1)
            {
                WriteLine($"{item} was not found. Please check your spelling.");
            }
            else
            {
                WriteLine($"'{name[location].Trim()}' was found while searching for {item}. Is this what you wanted?");
                string correct = ReadLine();
                if (correct.ToLower() == "y" || correct.ToLower() == "yes")
                {
                    Write("Enter how many you would like: ");
                    correct = ReadLine();
                    int.TryParse(correct, out int numItems);
                    if (correct == numItems.ToString() && numItems > -1)
                    {
                        order[location] = numItems;
                    }
                    else
                    {
                        WriteLine("That is not an acceptable answer.");
                    }
                }
                else
                {
                    WriteLine("I'm sorry I didn't find what you were looking for.");
                    WriteLine("Please try again after checking your spelling.");
                }
            }
            return order;
        }
        static int[] AddToOrder(string menu, int[] order)
        {
            WriteLine("What would you like?");
            string item = RecieveInput();
            
            if (item == "Order cancelled.")
            {
                order = CancelOrder(order);
            }
            else if (item == "Records checked.")
            {
                WriteLine("...");
                System.Threading.Thread.Sleep(1500);
                WriteLine("Files locked.");
            }
            else if (item == "Order complete.")
            {
                FinishOrder(menu, order);    
            }
            else
            {
                order = FillOrder(item, menu, order);
            }
            return order;
        }
        static int[] CancelOrder(int[] order)
        {
            for (var i = 0; i < order.Length; i++)
            {
                order[i] = 0;
            }
            return order;
        }

        static void FinishOrder(string menu, int[] order)
        {
            WriteLine("\nPrinting out your ticket...");
            System.Threading.Thread.Sleep(1500);
            PrintOrder(menu, order);
            
            decimal total = OrderTotal(menu, order);
            decimal tax = total*Manager.TaxRate;
            tax = (Math.Round((tax+0.005M)*100))/100;
            Manager.AddSaleToAllRecords("Daily", total, tax);

            total += tax;
            WriteLine($"Amount due: {total:C2}");
            Random ticket = new Random();
            WriteLine($"Ticket number: {ticket.Next(1000)}");
        }
        static decimal OrderTotal(string menu, int[] order)
        {
            decimal total = 0M;
            decimal tax = 0M;
            
            decimal[] menuPrices = new decimal[20];
            if (menu.ToLower() == "breakfast")
            {
                menuPrices = Breakfast_Menu.BreakfastPrices();
            }
            else if (menu.ToLower() == "lunch")
            {
                
            }
            else if (menu.ToLower() == "dinner")
            {
                
            }

            for (var i = 0; i < order.Length; i++)
            {
                int numItems = order[i];
                if (numItems != 0)
                {
                    total += menuPrices[i]*numItems;
                }
            }
            tax = total*Manager.TaxRate;
            tax = (Math.Round((tax+0.005M)*100))/100;
            WriteLine($"Tax ({(Manager.TaxRate):P})           {tax:C2}");
            return total;
        }
        static void PrintOrder(string menu, int[] currentOrder)
        {
            string[] useMenu = new string[20];
            decimal[] menuPrices = new decimal[20];
            if (menu.ToLower() == "breakfast")
            {
                useMenu = Breakfast_Menu.BreakfastItems();
                menuPrices = Breakfast_Menu.BreakfastPrices();
            }
            else if (menu.ToLower() == "lunch")
            {
                useMenu = Lunch_Menu.LunchItems();
                menuPrices = Lunch_Menu.LunchPrices();
            }
            else if (menu.ToLower() == "dinner")
            {
                useMenu = Dinner_Menu.DinnerItems();
                menuPrices = Dinner_Menu.DinnerPrices();
            }

            WriteLine("Your order:");
            for (var i = 0; i < currentOrder.Length; i++)
            {
                int numItems = currentOrder[i];
                if (numItems != 0)
                {
                    WriteLine($"{numItems}   {useMenu[i]}{(menuPrices[i]*numItems):C2}");
                }
            }
        }

        static string RecieveInput()
        {
            string item = ReadLine();
            if (item == "Master Override")
            {
                Manager.ManagerAccess();
                return "Records checked.";
            }
            else if (item.ToLower() == "exit" || item.ToLower() == "leave" || item.ToLower() == "cancel")
            {
                WriteLine("Have a great day!");
                return "Order cancelled.";
            }
            else if (item.ToLower() == "done" || item.ToLower() == "finish" || item.ToLower() == "pay")
            {
                WriteLine("Have a great day!");
                return "Order complete.";
            }
            else
            {
                return item;
            }
        }
    }
}
