namespace E_CommerceConsoleApp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    // Interface for shippable items
    public interface IShippable
    {
        string GetName();
        double GetWeight();
    }

    // Interface for perishable items
    public interface IPerishable
    {
        DateTime ExpiryDate { get; }
        bool IsExpired();
    }

    // Base Product class
    public abstract class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public Product(string name, decimal price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public virtual bool IsAvailable(int requestedQuantity)
        {
            return Quantity >= requestedQuantity && !IsExpired();
        }

        public virtual bool IsExpired()
        {
            return false;
        }
    }

    // Non-perishable product
    public class NonPerishableProduct : Product
    {
        public bool RequiresShipping { get; set; }
        public double Weight { get; set; } // in kg

        public NonPerishableProduct(string name, decimal price, int quantity, bool requiresShipping, double weight = 0)
            : base(name, price, quantity)
        {
            RequiresShipping = requiresShipping;
            Weight = weight;
        }
    }

    // Perishable product
    public class PerishableProduct : Product, IPerishable
    {
        public DateTime ExpiryDate { get; set; }
        public bool RequiresShipping { get; set; }
        public double Weight { get; set; } // in kg

        public PerishableProduct(string name, decimal price, int quantity, DateTime expiryDate, bool requiresShipping, double weight = 0)
            : base(name, price, quantity)
        {
            ExpiryDate = expiryDate;
            RequiresShipping = requiresShipping;
            Weight = weight;
        }

        public override bool IsExpired()
        {
            return DateTime.Now > ExpiryDate;
        }
    }

    // Cart Item class
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public decimal GetTotalPrice()
        {
            return Product.Price * Quantity;
        }
    }

    // Shopping Cart class
    public class ShoppingCart
    {
        public List<CartItem> Items { get; } = new List<CartItem>();

        public void Add(Product product, int quantity)
        {
            if (!product.IsAvailable(quantity))
            {
                throw new InvalidOperationException($"Product {product.Name} is not available in the requested quantity or has expired.");
            }

            var existingItem = Items.FirstOrDefault(item => item.Product == product);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem(product, quantity));
            }
        }

        public void Clear()
        {
            Items.Clear();
        }

        public decimal CalculateSubtotal()
        {
            return Items.Sum(item => item.GetTotalPrice());
        }

        public List<IShippable> GetShippableItems()
        {
            return Items
                .Where(item => item.Product is IShippable shippable &&
                              ((item.Product is NonPerishableProduct np && np.RequiresShipping) ||
                               (item.Product is PerishableProduct pp && pp.RequiresShipping)))
                .Select(item => item.Product as IShippable)
                .ToList();
        }
    }

    // Customer class
    public class Customer
    {
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public Customer(string name, decimal balance)
        {
            Name = name;
            Balance = balance;
        }

        public void DeductBalance(decimal amount)
        {
            if (Balance < amount)
            {
                throw new InvalidOperationException("Insufficient balance.");
            }
            Balance -= amount;
        }
    }

    // Shipping Service class
    public static class ShippingService
    {
        public static void ShipItems(List<IShippable> items)
        {
            if (items == null || items.Count == 0) return;

            Console.WriteLine("** Shipment notice **");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.GetName()}\t{item.GetWeight()}kg");
            }
            Console.WriteLine($"Total package weight {items.Sum(i => i.GetWeight())}kg");
            Console.WriteLine();
        }
    }

    // Checkout Service class
    public static class CheckoutService
    {
        public static void Checkout(Customer customer, ShoppingCart cart, decimal shippingFee = 30m)
        {
            if (cart.Items.Count == 0)
            {
                throw new InvalidOperationException("Cannot checkout with an empty cart.");
            }

            // Check for expired or out-of-stock items
            foreach (var item in cart.Items)
            {
                if (!item.Product.IsAvailable(item.Quantity))
                {
                    throw new InvalidOperationException($"Product {item.Product.Name} is not available in the requested quantity or has expired.");
                }
            }

            decimal subtotal = cart.CalculateSubtotal();
            decimal total = subtotal + shippingFee;

            // Process payment
            customer.DeductBalance(total);

            // Process shipping
            var shippableItems = cart.GetShippableItems();
            if (shippableItems.Count > 0)
            {
                ShippingService.ShipItems(shippableItems);
            }

            // Print receipt
            Console.WriteLine("** Checkout receipt **");
            foreach (var item in cart.Items)
            {
                Console.WriteLine($"{item.Quantity}x {item.Product.Name}\t{item.GetTotalPrice()}");
            }
            Console.WriteLine("---");
            Console.WriteLine($"Subtotal\t{subtotal}");
            Console.WriteLine($"Shipping\t{shippingFee}");
            Console.WriteLine($"Amount\t\t{total}");
            Console.WriteLine($"Customer balance after payment: {customer.Balance}");
            Console.WriteLine("END.");
            Console.WriteLine();

            // Update product quantities
            foreach (var item in cart.Items)
            {
                item.Product.Quantity -= item.Quantity;
            }

            cart.Clear();
        }
    }

    // Example usage
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

            // Test case 1: Example from the challenge
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

            // Test case 4: Out of stock
            Console.WriteLine("Test Case 4: Out of stock");
            var cart4 = new ShoppingCart();
            cart4.Add(biscuits, 10); // Only 5 available
            try
            {
                CheckoutService.Checkout(customer, cart4);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Test case 5: Empty cart
            Console.WriteLine("Test Case 5: Empty cart");
            var cart5 = new ShoppingCart();
            try
            {
                CheckoutService.Checkout(customer, cart5);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Test case 6: Mixed products with shipping
            Console.WriteLine("Test Case 6: Mixed products with shipping");
            customer.Balance = 20000m; // Add more balance
            var cart6 = new ShoppingCart();
            cart6.Add(cheese, 1);
            cart6.Add(tv, 1);
            cart6.Add(scratchCard, 2);
            try
            {
                CheckoutService.Checkout(customer, cart6);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
