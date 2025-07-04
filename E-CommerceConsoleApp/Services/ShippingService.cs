using E_CommerceConsoleApp.Model.Interfaces;

namespace E_CommerceConsoleApp.Services
{
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
}
