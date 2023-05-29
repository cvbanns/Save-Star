using WebApi.Entities;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
            return await _transactionRepository.CreateTransactionAsync(transaction);
        }

        public async Task<bool> DeleteTransactionAsync(Guid id)
        {
            return await _transactionRepository.DeleteTransactionAsync(id);
        }

        public async Task<List<Transaction>> GetSpending(Guid bankAccountId,DateTime startDate, DateTime endDate)
        {
            return await _transactionRepository.GetSpending( bankAccountId,startDate, endDate);
        }

        public async Task<Transaction> GetTransactionById(Guid id)
        {
            return await _transactionRepository.GetTransactionById(id);
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            return await _transactionRepository.GetTransactions();
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction)
        {
            return await _transactionRepository.UpdateTransactionAsync(transaction);
        }
    }
}
