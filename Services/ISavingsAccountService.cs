using WebApi.Entities;

namespace WebApi.Services
{
    public interface ISavingsAccountService
    {
        public Task<bool> CreateSavingsAccountAsync(SavingsAccount savingsaccount);
        public Task<SavingsAccount> GetSavingsAccountById(Guid id);
        public Task<bool> UpdateSavingsAccountAsync(SavingsAccount savingsaccount);
        Task<List<SavingsAccount>> GetSavingsAccounts();
    }
}
