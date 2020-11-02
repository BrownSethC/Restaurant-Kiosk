using System;
using System.IO;
using static System.Console;

class Menu
{
    public static decimal[] ChangePrice(int location, decimal[] menuPrices, decimal newPrice, string menu)
    {
        WriteLine($"Replacing {menuPrices[location]:C2} with {newPrice:C2} now...");
        string text = "";
        menu = menu.ToLower();

        for (var i = 1; i < menuPrices.Length; i++)
        {
            var item = menuPrices[i];
            if (i == location)
            {
                text += "-" + newPrice.ToString();
                menuPrices[i] = newPrice;
            }
            else
            {
                text += "-" + item.ToString();
            }
            
        }

        if (menu == "breakfast")
        {
            File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Breakfast Prices.txt", text);
        }
        else if (menu == "lunch")
        {
            File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Lunch Prices.txt", text);
        }
        else if (menu == "dinner")
        {
            File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Dinner Prices.txt", text);
        }
        WriteLine("The price has been adjusted.");
        return menuPrices;
    }

    public static string[] ChangeName(int location, string[] menuItems, string newName, string menu)
    {
        WriteLine($"Replacing {menuItems[location].Trim()} with {newName} now...");
        string text = "";
        menu = menu.ToLower();
        int size = 0;
        foreach (var item in menuItems)
        {
            if (item.Length > size)
            {
                size = item.Length;
            }
        }
        size += 5;

        for (var i = 1; i < menuItems.Length; i++)
        {
            int numSpaces = 0;
            var item = menuItems[i].Trim();
            string space = "";
            if (i == location)
            {
                numSpaces = size-newName.Length;
                if (numSpaces == 0)
                {
                    size += 5;
                    numSpaces = 5;
                }
                for (var j = 0; j < numSpaces; j++)
                {
                    space += " ";
                }
                text += "-" + newName + space;
                menuItems[i] = newName + space;
            }
            else
            {
                space = "";
                numSpaces = size-item.Length;
                if (numSpaces == 0)
                {
                    size += 5;
                    numSpaces = 5;
                }
                for (var j = 0; j < numSpaces; j++)
                {
                    space += " ";
                }
                text += "-" + item + space;
            }
            
        }

        if (menu == "breakfast")
        {
            File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Breakfast Items.txt", text);
        }
        else if (menu == "lunch")
        {
            File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Lunch Items.txt", text);
        }
        else if (menu == "dinner")
        {
            File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Dinner Items.txt", text);
        }
        WriteLine("The name has been changed.");
        return menuItems;
    }
    
    public static void AddToMenu(string menuName)
    {
        menuName = menuName.ToLower();
        
        Write("Enter the item you would like to add: ");
        string nameItem = ReadLine();
        if (nameItem != "")
        {
            Write($"Enter the price for {nameItem}: $");
            decimal.TryParse(ReadLine(), out decimal itemPrice);
            if (itemPrice <= 0)
            {
                WriteLine("This is not an acceptable price. Please try again.");
            }
            else
            {
                WriteLine($"{nameItem} is {itemPrice:C2}");
                if (menuName == "breakfast")
                {
                    Breakfast_Menu.AddToMenu(itemPrice, nameItem);
                }
                else if (menuName == "lunch")
                {
                    Lunch_Menu.AddToMenu(itemPrice, nameItem);
                }
                else if (menuName == "dinner")
                {
                    Dinner_Menu.AddToMenu(itemPrice, nameItem);
                }
            }
        }
        else
        {
            WriteLine("You did not enter an item to add.");
        }
    }
    public static void RemoveFromMenu(string menuName)
    {
        menuName = menuName.ToLower();

        Write("Enter the item you would like to remove from the menu: ");
        string nameItem = ReadLine();
        if (nameItem != "")
        {
            WriteLine($"{nameItem} is being removed.");
            if (menuName == "breakfast")
            {
                Breakfast_Menu.RemoveFromMenu(nameItem);
            }
            else if (menuName == "lunch")
            {
                Lunch_Menu.RemoveFromMenu(nameItem);
            }
            else if (menuName == "dinner")
            {
                Dinner_Menu.RemoveFromMenu(nameItem);
            }
        }
    }
    public static void EditMenuItem(string menuName)
    {
        menuName = menuName.ToLower();
        string[] menu = FindMenu(menuName);
        string[] menuItems = FindMenuItems(menuName);
        decimal[] menuPrices = FindMenuPrices(menuName);
        bool doneEditing = false;

        while (!doneEditing)
        {
            menu = FindMenu(menuName);
            menuItems = FindMenuItems(menuName);
            menuPrices = FindMenuPrices(menuName);
            PrintOutMenu(menu);

            WriteLine("\nEnter the item would you like to edit, or 1 to return to the editing menu.");
            string itemToEdit = ReadLine();
            if (itemToEdit == "1")
            {
                break;
            }
            int location = FindInMenu(itemToEdit, menuName);
            if (location == -1)
            {
                WriteLine("That item was not located in the menu. Check spelling.");
            }
            else
            {
                WriteLine("\nEnter 1 to edit the name of the item.");
                WriteLine("Enter 2 to edit the price of the item.");
                WriteLine("Enter anything else to return to the editing menu.");
                string choice = ReadLine();

                if (choice == "1")
                {
                    Write("Enter the new or corrected name: ");
                    choice = ReadLine();
                    ChangeName(location, menuItems, choice, menuName);
                }
                else if (choice == "2")
                {
                    Write("Enter the new price: $");
                    choice = ReadLine();
                    decimal.TryParse(choice, out decimal newPrice);
                    if (newPrice.ToString() != choice)
                    {
                        WriteLine("You did not enter a number. Price was not changed.");
                    }
                    else if (newPrice > menuPrices[location]*3)
                    {
                        WriteLine("You are more than tripling the old price. Enter 1 to confirm this action: ");
                        choice = ReadLine();
                        if (choice == "1")
                        {
                            menuPrices = ChangePrice(location, menuPrices, newPrice, menuName);
                            WriteLine($"The new price of {menuItems[location]} is {newPrice}");
                        }
                        else
                        {
                            WriteLine("The price was not changed.");
                        }
                    }
                    else if (newPrice < 0.25M && newPrice >= 0)
                    {
                        WriteLine("The price will be very low. Enter 1 to confirm this action: ");
                        choice = ReadLine();
                        if (choice == "1")
                        {
                            menuPrices = ChangePrice(location, menuPrices, newPrice, menuName);
                            WriteLine($"The new price of {menuItems[location]} is {newPrice}");
                        }
                        else
                        {
                            WriteLine("The price was not changed.");
                        }
                    }
                    else if (newPrice < 0)
                    {
                        WriteLine("That is not an acceptable price change. Price was not changed.");
                    }
                    else
                    {
                        menuPrices = ChangePrice(location, menuPrices, newPrice, menuName);
                        WriteLine($"The new price of the {menuItems[location].ToLower().Trim()} is {newPrice}");
                    }
                }
                else
                {
                    doneEditing = true;
                }
            }
        }
    }
    public static int FindInMenu(string itemName, string menu)
    {
        menu = menu.ToLower();
        if (itemName == "")
        {
            WriteLine("You did not select an item to find.");
            return -1;
        }
        string[] checkMenu = FindMenuItems(menu);
        int spot = 0;
        
        foreach (var item in checkMenu)
        {
            if (item.ToLower() == itemName.ToLower() || item.ToLower().Contains(itemName.ToLower()))
            {
                return spot;
            }
            else
            {
                spot++;
            }
        }
        return -1;
    }
    public static string[] FindMenuItems(string menu)
    {
        menu = menu.ToLower();
        string[] checkMenu = new string[20];
        if (menu == "breakfast")
        {
            checkMenu = Breakfast_Menu.BreakfastItems();
        }
        else if (menu == "lunch")
        {
            checkMenu = Lunch_Menu.LunchItems();
        }
        else if (menu == "dinner")
        {
            checkMenu = Dinner_Menu.DinnerItems();
        }
        return checkMenu;
    }
    public static decimal[] FindMenuPrices(string menu)
    {
        menu = menu.ToLower();
        decimal[] checkMenu = new decimal[20];
        if (menu == "breakfast")
        {
            checkMenu = Breakfast_Menu.BreakfastPrices();
        }
        else if (menu == "lunch")
        {
            checkMenu = Lunch_Menu.LunchPrices();
        }
        else if (menu == "dinner")
        {
            checkMenu = Dinner_Menu.DinnerPrices();
        }
        return checkMenu;
    }
    public static string[] FindMenu(string menu)
    {
        string[] checkMenu = new string[20];
        menu = menu.ToLower();
        if (menu == "breakfast")
        {
            checkMenu = Breakfast_Menu.Breakfast();
        }
        else if (menu == "lunch")
        {
            checkMenu = Lunch_Menu.Lunch();
        }
        else if (menu == "dinner")
        {
            checkMenu = Dinner_Menu.Dinner();
        }
        return checkMenu;
    }
    public static void PrintOutMenu(string[] menu)
    {
        WriteLine("\nMenu Item         Cost\n");
        foreach (var item in menu)
        {
            WriteLine(item);
        }
    }
}