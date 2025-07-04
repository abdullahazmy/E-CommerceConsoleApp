using E_CommerceConsoleApp.Model;

namespace E_CommerceConsoleApp.Services
{
    public static class CheckoutService
    {
        public static void Checkout(Customer customer, ShoppingCart cart, decimal shippingFee = 30m)
        {
            if (cart.Items.Count == 0)
            {
                throw new InvalidOperationException("Cannot checkout with an empty cart.");
            }

            foreach (var item in cart.Items)
            {
                if (!item.Product.IsAvailable(item.Quantity))
                {
                    throw new InvalidOperationException($"Product {item.Product.Name} is not available in the requested quantity or has expired.");
                }
            }

            decimal subtotal = cart.CalculateSubtotal();
            decimal total = subtotal + shippingFee;

            customer.DeductBalance(total);

            var shippableItems = cart.GetShippableItems();
            if (shippableItems.Count > 0)
            {
                ShippingService.ShipItems(shippableItems);
            }

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

            foreach (var item in cart.Items)
            {
                item.Product.Quantity -= item.Quantity;
            }

            cart.Clear();
        }
    }
}
