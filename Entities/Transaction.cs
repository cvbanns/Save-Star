using WebApi.Enums;

namespace WebApi.Entities
{
    public class Transaction : BaseEntity
    {
        public Transaction(Guid bankAccountId, double amount, TransactionType transactionType)
        {
            BankAccountId = bankAccountId;
            Amount = amount;
            TransactionType = transactionType;
        }

        public double Amount { get; set; }
        public TransactionType TransactionType { get; set; }
        public BankAccount BankAccount { get; set; }
        public Guid BankAccountId { get; set; }

    }
}

