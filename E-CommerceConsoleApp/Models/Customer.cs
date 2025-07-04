namespace E_CommerceConsoleApp.Model
{
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
}
