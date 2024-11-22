// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    // Class representing a product
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public Product(string name, decimal price, int stock)
        {
            Name = name;
            Price = price;
            Stock = stock;
        }

        public override string ToString()
        {
            return $"{Name} - ${Price} - {Stock} in stock";
        }
    }

    // List to hold the inventory
    static List<Product> inventory = new List<Product>();

    static void Main(string[] args)
    {
        LoadInventory();
        DisplayMenu();
    }

    // Display the main menu
    static void DisplayMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== Inventory Management System ===");
            Console.WriteLine("1. Add New Product");
            Console.WriteLine("2. Update Product Stock");
            Console.WriteLine("3. View All Products");
            Console.WriteLine("4. Remove Product");
            Console.WriteLine("5. Search Product");
            Console.WriteLine("6. Exit");
            Console.WriteLine("===================================");

            Console.Write("Select an option (1-6): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddNewProduct();
                    break;
                case "2":
                    UpdateProductStock();
                    break;
                case "3":
                    ViewAllProducts();
                    break;
                case "4":
                    RemoveProduct();
                    break;
                case "5":
                    SearchProduct();
                    break;
                case "6":
                    exit = true;
                    SaveInventory();
                    Console.WriteLine("Exiting... Inventory saved.");
                    break;
                default:
                    Console.WriteLine("Invalid option, please try again.");
                    break;
            }
        }
    }

    // Add a new product to the inventory
    static void AddNewProduct()
    {
        Console.Clear();
        Console.WriteLine("=== Add New Product ===");
        
        Console.Write("Enter Product Name: ");
        string name = Console.ReadLine();

        decimal price;
        while (true)
        {
            Console.Write("Enter Product Price: ");
            if (decimal.TryParse(Console.ReadLine(), out price) && price > 0)
                break;
            else
                Console.WriteLine("Invalid input. Please enter a valid price.");
        }

        int stock;
        while (true)
        {
            Console.Write("Enter Stock Quantity: ");
            if (int.TryParse(Console.ReadLine(), out stock) && stock >= 0)
                break;
            else
                Console.WriteLine("Invalid input. Please enter a valid stock quantity.");
        }

        // Create a new product and add it to the inventory
        inventory.Add(new Product(name, price, stock));
        Console.WriteLine("Product added successfully!");
        Console.ReadLine();
    }

    // Update stock of an existing product
    static void UpdateProductStock()
    {
        Console.Clear();
        Console.WriteLine("=== Update Product Stock ===");

        Console.Write("Enter Product Name to Update: ");
        string name = Console.ReadLine();

        Product product = inventory.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (product != null)
        {
            int quantityChange;
            while (true)
            {
                Console.Write("Enter Quantity to Add/Remove (negative for sold items): ");
                if (int.TryParse(Console.ReadLine(), out quantityChange))
                    break;
                else
                    Console.WriteLine("Invalid input. Please enter a valid quantity.");
            }

            product.Stock += quantityChange;

            if (product.Stock < 0)
                product.Stock = 0; // Prevent negative stock.

            Console.WriteLine("Stock updated successfully!");
        }
        else
        {
            Console.WriteLine("Product not found.");
        }
        Console.ReadLine();
    }

    // View all products in inventory
    static void ViewAllProducts()
    {
        Console.Clear();
        Console.WriteLine("=== All Products ===");

        if (inventory.Count == 0)
        {
            Console.WriteLine("No products in inventory.");
        }
        else
        {
            foreach (var product in inventory)
            {
                Console.WriteLine(product);
            }
        }

        Console.ReadLine();
    }

    // Remove a product from inventory
    static void RemoveProduct()
    {
        Console.Clear();
        Console.WriteLine("=== Remove Product ===");

        Console.Write("Enter Product Name to Remove: ");
        string name = Console.ReadLine();

        Product product = inventory.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (product != null)
        {
            inventory.Remove(product);
            Console.WriteLine("Product removed successfully!");
        }
        else
        {
            Console.WriteLine("Product not found.");
        }

        Console.ReadLine();
    }

    // Search for a product by name
    static void SearchProduct()
    {
        Console.Clear();
        Console.WriteLine("=== Search Product ===");

        Console.Write("Enter Product Name to Search: ");
        string name = Console.ReadLine();

        Product product = inventory.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        if (product != null)
        {
            Console.WriteLine("Product found:");
            Console.WriteLine(product);
        }
        else
        {
            Console.WriteLine("Product not found.");
        }

        Console.ReadLine();
    }

    // Load inventory from a file (CSV format)
    static void LoadInventory()
    {
        if (File.Exists("inventory.csv"))
        {
            var lines = File.ReadAllLines("inventory.csv");
            foreach (var line in lines)
            {
                var columns = line.Split(',');
                if (columns.Length == 3)
                {
                    string name = columns[0];
                    decimal price = decimal.Parse(columns[1]);
                    int stock = int.Parse(columns[2]);

                    inventory.Add(new Product(name, price, stock));
                }
            }
        }
    }

    // Save inventory to a file (CSV format)
    static void SaveInventory()
    {
        using (StreamWriter writer = new StreamWriter("inventory.csv"))
        {
            foreach (var product in inventory)
            {
                writer.WriteLine($"{product.Name},{product.Price},{product.Stock}");
            }
        }
    }
}
