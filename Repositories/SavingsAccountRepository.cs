
using Microsoft.EntityFrameworkCore;
using System;
using WebApi.Entities;
using WebApi.Enums;
using WebApi.Helpers;

namespace WebApi.Repositories
{
    public class SavingsAccountRepository : ISavingsAccountRepository
    {
        private readonly DataContext _dataContext;

        public SavingsAccountRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateSavingsAccountAsync(SavingsAccount savingsAccount)
        {
           await _dataContext.SavingsAccounts.AddAsync(savingsAccount);
           return await _dataContext.SaveChangesAsync() != 0;
        }

        public async Task<bool> CreditSavingsAccountAsync(Guid savingsAccountId, double amount)
        {
            var savingsAccount = await GetSavingsAccountById(savingsAccountId);
            savingsAccount.Balance += amount;
            return await UpdateSavingsAccountAsync(savingsAccount);
        }

        public async Task<SavingsAccount> GetSavingsAccountById(Guid id)
        {
            return await _dataContext.SavingsAccounts.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        public async Task<List<SavingsAccount>> GetSavingsAccounts()
        {
            return await _dataContext.SavingsAccounts.ToListAsync();
        }

        public async Task<bool> UpdateSavingsAccountAsync(SavingsAccount savingsaccount)
        {
            _dataContext.Update(savingsaccount);
            return await _dataContext.SaveChangesAsync() != 0;
        }

    }
}
