namespace OnlineRetailersStore.Data.Models
{
    public class UserInformation
    {
        public User User { get; set; } = new User();
        public Address Address { get; set; } = new Address();
        public BankAccount BankAccount { get; set; } = new BankAccount();
    }
}
