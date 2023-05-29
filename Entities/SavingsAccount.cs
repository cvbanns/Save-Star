namespace WebApi.Entities
{
    public class SavingsAccount : BaseEntity
    {
        public double Balance { get; set; }
        public Account Account { get; set; }
        public Guid AccountId { get; set; }
    }
}
