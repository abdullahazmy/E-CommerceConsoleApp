using E_CommerceConsoleApp.Model;
using E_CommerceConsoleApp.Model.Interfaces;

namespace E_CommerceConsoleApp.Services
{
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
}
