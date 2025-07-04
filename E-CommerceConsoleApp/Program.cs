using E_CommerceConsoleApp.Model;
using E_CommerceConsoleApp.Services;

namespace E_CommerceConsoleApp
{

    class Program
    {
        static void Main(string[] args)
        {
            // Create products
            var cheese = new PerishableProduct("Cheese", 100m, 10, DateTime.Now.AddDays(7), true, 0.4);
            var biscuits = new PerishableProduct("Biscuits", 150m, 5, DateTime.Now.AddDays(14), true, 0.7);
            var tv = new NonPerishableProduct("TV", 10000m, 3, true, 15.5);
            var scratchCard = new NonPerishableProduct("Mobile Scratch Card", 50m, 100, false);

            // Create customer
            var customer = new Customer("John Doe", 500m);

            // Test case: Example from the challenge
            Console.WriteLine("Test Case 1: Example from the challenge");
            var cart1 = new ShoppingCart();
            cart1.Add(cheese, 2);
            cart1.Add(biscuits, 1);
            try
            {
                CheckoutService.Checkout(customer, cart1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }


            // Test case 2: Insufficient balance
            Console.WriteLine("Test Case 2: Insufficient balance");
            var cart2 = new ShoppingCart();
            cart2.Add(tv, 1);
            try
            {
                CheckoutService.Checkout(customer, cart2);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Test case 3: Expired product
            Console.WriteLine("Test Case 3: Expired product");
            var expiredCheese = new PerishableProduct("Expired Cheese", 100m, 5, DateTime.Now.AddDays(-1), true, 0.4);
            var cart3 = new ShoppingCart();
            cart3.Add(expiredCheese, 1);
            try
            {
                CheckoutService.Checkout(customer, cart3);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

        }
    }
}

