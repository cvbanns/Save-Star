using WebApi.Entities;

namespace WebApi.Services
{
    public interface ITransactionService
    {
        public Task<bool> CreateTransactionAsync(Transaction transaction);
        public Task<Transaction> GetTransactionById(Guid id);
        public Task<bool> UpdateTransactionAsync(Transaction transaction);
        public Task<bool> DeleteTransactionAsync(Guid id);
        Task<List<Transaction>> GetTransactions();
        public Task<List<Transaction>> GetSpending(Guid bankAccountId, DateTime startDate, DateTime endDate);
    }
}
