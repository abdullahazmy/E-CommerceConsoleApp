using E_CommerceConsoleApp.Models.Interfaces;

namespace E_CommerceConsoleApp.Model
{
    public class PerishableProduct : Product, IPerishable
    {
        public DateTime ExpiryDate { get; set; }
        public bool RequiresShipping { get; set; }
        public double Weight { get; set; } // in kg

        public PerishableProduct(string name, decimal price, int quantity,
                               DateTime expiryDate, bool requiresShipping, double weight = 0)
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
}
