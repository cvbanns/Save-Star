
using Microsoft.EntityFrameworkCore;
using System;
using WebApi.Entities;
using WebApi.Enums;
using WebApi.Helpers;

namespace WebApi.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly DataContext _dataContext;

        public TransactionRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateTransactionAsync(Transaction transaction)
        {
            var bankAccount = await _dataContext.BankAccounts.FirstOrDefaultAsync(x => x.Id == transaction.BankAccountId);
            if(transaction.TransactionType == TransactionType.Credit)
            {
                bankAccount.Balance += transaction.Amount;
            }
            bankAccount.Balance -= transaction.Amount;
            await _dataContext.Transactions.AddAsync(transaction);
            return await _dataContext.SaveChangesAsync() !=0;
        }

        public async Task<bool> DeleteTransactionAsync(Guid id)
        {
            var transaction = await GetTransactionById(id);
            if (transaction != null)
            {
                transaction.IsDeleted = true;
            }
            return await _dataContext.SaveChangesAsync() != 0;

        }

        public async Task<List<Transaction>> GetSpending(Guid bankAccountId, DateTime startDate, DateTime endDate) //front end handles the date, it accepts compile time value
        {
           return  await _dataContext.Transactions
                .Include(x => x.BankAccount)
                .Where(x => x.TransactionType == TransactionType.Debit 
                    && x.BankAccountId == bankAccountId
                    && ( x.CreatedAt >= startDate)
                    && (x.CreatedAt <= endDate)).ToListAsync();
        }

        public async Task<Transaction> GetTransactionById(Guid id)
        {
            return await _dataContext.Transactions.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<Transaction>> GetTransactions()
        {
            return await _dataContext.Transactions.ToListAsync();
        }

        public async Task<bool> UpdateTransactionAsync(Transaction transaction)
        {
            _dataContext.Update(transaction);
            return await _dataContext.SaveChangesAsync() != 0;
        }

    }
}
