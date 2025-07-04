namespace E_CommerceConsoleApp.Models.Interfaces
{
    public interface IPerishable
    {
        DateTime ExpiryDate { get; }
        bool IsExpired();
    }
}
