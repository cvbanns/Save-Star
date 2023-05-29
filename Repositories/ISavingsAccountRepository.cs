
using WebApi.Entities;
using WebApi.Enums;

namespace WebApi.Repositories
{
    public interface ISavingsAccountRepository
    {
        public Task<bool> CreateSavingsAccountAsync(SavingsAccount account);
        public Task<SavingsAccount> GetSavingsAccountById(Guid id);
        public Task<bool> UpdateSavingsAccountAsync(SavingsAccount savingsaccount);
        public Task<bool> CreditSavingsAccountAsync(Guid savingsAccountId, double Amount);
        Task<List<SavingsAccount>> GetSavingsAccounts();
    }
}