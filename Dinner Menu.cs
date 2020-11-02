using System;
using System.IO;
using static System.Console;

class Dinner_Menu
{
    public static void AddToMenu(decimal price, string itemName)
    {
        string[] oldMenu = DinnerItems();
        decimal[] oldPrices = DinnerPrices();

        string[] newMenu = new string[oldMenu.Length+1];
        decimal[] newPrices = new decimal[oldPrices.Length+1];
        for (var i = 0; i < oldMenu.Length; i++)
        {
            newMenu[i] = oldMenu[i];
            newPrices[i] = oldPrices[i];
        }
        newMenu[newMenu.Length-1] = itemName;
        newPrices[newPrices.Length-1] = price;

        string prices = "";
        string items = "";

        int size = 0;
        foreach (var item in newMenu)
        {
            if (item.Length > size)
            {
                size = item.Length;
            }
        }
        size += 5;

        int numSpaces = 0;
        string space = "";
        
        for (var i = 1; i < newPrices.Length; i++)
        {
            numSpaces = size - (newMenu[i].Length);
            for (var j = 0; j < numSpaces; j++)
            {
                space += " ";
            }
            var item = newPrices[i].ToString();
            prices += "-" + item;
            item = newMenu[i];
            items += "-" + item + space;
        }
        
        File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Dinner Prices.txt", prices);
        File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Dinner Items.txt", items);
    }
    public static void RemoveFromMenu(string itemName)
    {
        int location = Menu.FindInMenu(itemName, "Dinner");
        if (location == -1)
        {
            WriteLine("Item to remove was not found in the menu. Please check your spelling.");
        }
        else
        {
            string[] oldMenu = DinnerItems();
            decimal[] oldPrices = DinnerPrices();
            WriteLine($"{oldMenu[location]} was found while looking for {itemName}.");
            Write("If this is correct, enter 1: ");
            if (ReadLine() == "1")
            {    
                string[] newMenu = new string[oldMenu.Length];
                decimal[] newPrices = new decimal[oldPrices.Length];
                for (var i = 0; i < (oldMenu.Length-1); i++)
                {
                    if (i != location)
                    {
                        newMenu[i] = oldMenu[i];
                        newPrices[i] = oldPrices[i];
                    }
                    else
                    {
                        newPrices[i] = 0;
                    }
                }
                
                string prices = "";
                string items = "";
                int size = 0;
                foreach (var item in newMenu)
                {
                    if (item.Length > size)
                    {
                        size = item.Length;
                    }
                }
                size += 5;

                int numSpaces = 0;
                string space = "";
                
                for (var i = 1; i < newPrices.Length; i++)
                {
                    if (newPrices[i] != 0)
                    {
                        numSpaces = size - (newMenu[i].Length);
                    for (var j = 0; j < numSpaces; j++)
                    {
                        space += " ";
                    }
                    var item = newPrices[i].ToString();
                    prices += "-" + item;
                    item = newMenu[i];
                    items += "-" + item + space;
                    }
                }
                
                File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Dinner Prices.txt", prices);
                File.WriteAllText(@"C:\Users\Kiosk Records\Menus and Prices\Dinner Items.txt", items);
            }
        }
    }
    public static void PrintOutMenu()
    {
        string[] menu = Dinner();
        WriteLine("Menu Item         Cost\n");
        foreach (var item in menu)
        {
            if (item != "")
            {
                WriteLine(item);
            }
        }
    }
    public static string[] DinnerItems()
    {
        string text = File.ReadAllText(@"C:\Users\Kiosk Records\Menus and Prices\Dinner Items.txt");
        string[] items = text.Split("-");
        
        return items;
    }
    public static decimal[] DinnerPrices()
    {
        string text = File.ReadAllText(@"C:\Users\Kiosk Records\Menus and Prices\Dinner Prices.txt");
        string[] items = text.Split("-");
        decimal[] prices = new decimal[items.Length];
        
        for (var i = 0; i < items.Length; i++)
        {
            decimal.TryParse(items[i], out prices[i]);
        }

        return prices;
    }
    public static string[] Dinner()
    {
        string[] items = DinnerItems();
        decimal[] prices = DinnerPrices();
        string[] menu = new string[items.Length-1];
        
        for (var i = 1; i < items.Length; i++)
        {
            if(prices[i] != 0)
            {
                menu[i-1] = $"{items[i]}{prices[i]:C2}";
            }
            else
            {
                menu[i] = "";
            }
        }
        return menu;
    }
}