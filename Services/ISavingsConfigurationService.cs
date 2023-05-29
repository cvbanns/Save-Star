using WebApi.Entities;

namespace WebApi.Services
{
    public interface ISavingsConfigurationService
    {
        public Task<List<SavingsConfiguration>> GetSavingsConfiguration();
        public Task<bool> CreateSavingsConfiguration(SavingsConfiguration savingsConfiguration);
        Task ProcessSavings();
    }
}
