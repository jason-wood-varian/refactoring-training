using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        private static List<User> Users { get; set; }
        private static List<Product> Products { get; set; }
        private static User ActiveUser { get; set; }

        public static void Start(List<User> users, List<Product> products)
        {
            Users = users;
            Products = products;

            WriteWelcomeMessage();
            bool loggedIn;
            do
            {
                loggedIn = Login();
            } while (!loggedIn);

            ShowRemainingBalance();

            // Show product list
            while (true)
            {
                PurchaseItem();
                

                // Check if user entered number that equals product count
                if (number == 7)
                {
                    // Update balance
                    foreach (var user in users)
                    {
                        // Check that name and password match
                        if (user.Name == ActiveUser.Name && user.Password == ActiveUser.Password)
                        {
                            user.Balance = ActiveUser.Balance;
                        }
                    }

                    // Write out new balance
                    string json = JsonConvert.SerializeObject(users, Formatting.Indented);
                    File.WriteAllText(@"Data\Users.json", json);

                    // Write out new quantities
                    string json2 = JsonConvert.SerializeObject(products, Formatting.Indented);
                    File.WriteAllText(@"Data\Products.json", json2);


                    // Prevent console from closing
                    Console.WriteLine();
                    Console.WriteLine("Press Enter key to exit");
                    Console.ReadLine();
                    return;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("You want to buy: " + products[number].Name);
                    Console.WriteLine("Your balance is " + ActiveUser.Balance.ToString("C"));

                    // Prompt for user input
                    Console.WriteLine("Enter amount to purchase:");
                    answer = Console.ReadLine();
                    int quantity = Convert.ToInt32(answer);

                    // Check if balance - quantity * price is less than 0
                    if (remainingBalance - products[number].Price * quantity < 0)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("You do not have enough money to buy that.");
                        Console.ResetColor();
                        continue;
                    }

                    // Check if quantity is less than quantity
                    if (products[number].Quantity <= quantity)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("Sorry, " + products[number].Name + " is out of stock");
                        Console.ResetColor();
                        continue;
                    }

                    // Check if quantity is greater than zero
                    if (quantity > 0)
                    {
                        // Balance = Balance - Price * Quantity
                        remainingBalance = remainingBalance - products[number].Price * quantity;

                        // Quanity = Quantity - Quantity
                        products[number].Quantity = products[number].Quantity - quantity;

                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("You bought " + quantity + " " + products[number].Name);
                        Console.WriteLine("Your new balance is " + remainingBalance.ToString("C"));
                        Console.ResetColor();
                    }
                    else
                    {
                        // Quantity is less than zero
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine();
                        Console.WriteLine("Purchase cancelled");
                        Console.ResetColor();
                    }
                }
            }
            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }

        private static void ShowRemainingBalance()
        {
            Console.WriteLine();
            Console.WriteLine("Your balance is " + ActiveUser.Balance.ToString("C"));
        }

        private static void WriteWelcomeMessage()
        {
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
        }

        private static bool Login()
        {
            if (!ValidateUser())
            {
                WriteInvalidUserMessage();
                return false;
            }
            if (!ValidatePassword())
            {
                WriteInvalidPasswordMessage();
                return false;
            }
            WriteSuccessfulLoginMessage();
            return true;
        }

        private static bool ValidateUser()
        {
            var username = PromptForUsername();
            return IsUserValid(username);
        }

        private static string PromptForUsername()
        {
            Console.WriteLine();
            Console.WriteLine("Enter Username:");
            var username = Console.ReadLine();
            return username;
        }

        private static bool IsUserValid(string username)
        {
            if (string.IsNullOrEmpty(username))
                return false;

            for (int i = 0; i < Users.Count; i++)
            {
                User user = Users[i];
                if (user.Name == username)
                {
                    ActiveUser.Name = username;
                    return true;
                }
            }

            return false;
        }

        private static void WriteInvalidUserMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid user.");
            Console.ResetColor();
        }

        private static bool ValidatePassword()
        {
            var password = PromptForPassword();
            return IsPasswordValid(password);
        }

        private static string PromptForPassword()
        {
            Console.WriteLine("Enter Password:");
            var password = Console.ReadLine();
            return password;
        }

        private static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password))
                return false;

            for (int i = 0; i < Users.Count; i++)
            {
                User user = Users[i];
                if (user.Name == ActiveUser.Name && user.Password == password)
                {
                    ActiveUser.Password = password;
                    return true;
                }
            }

            return false;
        }

        private static void WriteInvalidPasswordMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid password.");
            Console.ResetColor();
        }

        private static void WriteSuccessfulLoginMessage()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Login successful! Welcome " + ActiveUser.Name + "!");
            Console.ResetColor();
        }

        private static void PurchaseItem()
        {
            ShowProducts();
            PromptForProductNumber();
        }

        private static void ShowProducts()
        {
            Console.WriteLine();
            Console.WriteLine("What would you like to buy?");
            for (int i = 0; i < Products.Count; i++)
            {
                Product product = Products[i];
                Console.WriteLine(i + 1 + ": " + product.Name + " (" + product.Price.ToString("C") + ")");
            }
            Console.WriteLine(Products.Count + 1 + ": Exit");
        }

        private static void PromptForProductNumber()
        {
            Console.WriteLine("Enter a number:");
            string answer = Console.ReadLine();
            int number = Convert.ToInt32(answer);
            number = number - 1; /* Subtract 1 from number
            num = num + 1 // Add 1 to number */
        }
    }
}
