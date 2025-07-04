namespace E_CommerceConsoleApp.Model
{
    public class NonPerishableProduct : Product
    {
        public bool RequiresShipping { get; set; }
        public double Weight { get; set; } // in kg

        public NonPerishableProduct(string name, decimal price, int quantity,
                                  bool requiresShipping, double weight = 0)
            : base(name, price, quantity)
        {
            RequiresShipping = requiresShipping;
            Weight = weight;
        }
    }
}
