namespace OnlineRetailersStore.Data.Models
{
    public class CustomerInformation
    {
        public Customer Customer { get; set; } = new Customer();
        public Address Address { get; set; } = new Address();
        public BankAccount BankAccount { get; set; } = new BankAccount(); 
    }
}
