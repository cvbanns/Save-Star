using WebApi.Entities;

namespace WebApi.Repositories
{
    public interface ISavingsConfigurationRepository
    {
        public Task<List<SavingsConfiguration>> GetSavingsConfiguration();
        public Task<bool> CreateSavingsConfiguration(SavingsConfiguration savingsConfiguration);
    }
}
