using WebApi.Entities;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class SavingsAccountService : ISavingsAccountService
    {
        private readonly ISavingsAccountRepository _savingsaccountRepository;

        public SavingsAccountService(ISavingsAccountRepository savingsaccountRepository)
        {
            _savingsaccountRepository = savingsaccountRepository;
        }

        public async Task<bool> CreateSavingsAccountAsync(SavingsAccount savingsaccount)
        {
            return await _savingsaccountRepository.CreateSavingsAccountAsync(savingsaccount);
        }

        public async Task<SavingsAccount> GetSavingsAccountById(Guid id)
        {
            return await _savingsaccountRepository.GetSavingsAccountById(id);
        }

        public async Task<List<SavingsAccount>> GetSavingsAccounts()
        {
            return await _savingsaccountRepository.GetSavingsAccounts();
        }

        public async Task<bool> UpdateSavingsAccountAsync(SavingsAccount savingsaccount)
        {
            return await _savingsaccountRepository.UpdateSavingsAccountAsync(savingsaccount);
        }
    }
}
