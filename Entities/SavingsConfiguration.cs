using WebApi.Enums;

namespace WebApi.Entities
{
    public class SavingsConfiguration : BaseEntity
    {
        public BankAccount BankAccount { get; set; }
        public Guid BankAccountId { get; set; }
        public SavingsAccount SavingsAccount { get; set; }
        public Guid SavingsAccountId { get; set; }
        public SavingsType SavingsType { get; set; }
        public PeriodType PeriodType { get; set; }
        public double Percentage { get; set; }
        public double FixedSaving { get; set; }
        public DateTime StartDate { get; set; }
    }
}
