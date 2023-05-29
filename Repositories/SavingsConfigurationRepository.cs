using Microsoft.EntityFrameworkCore;
using WebApi.Entities;
using WebApi.Helpers;

namespace WebApi.Repositories
{
    public class SavingsConfigurationRepository : ISavingsConfigurationRepository
    {
        private readonly DataContext _dataContext;

        public SavingsConfigurationRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<bool> CreateSavingsConfiguration(SavingsConfiguration savingsConfiguration)
        {
            await _dataContext.SavingsConfigurations.AddAsync(savingsConfiguration);
            return _dataContext.SaveChanges() !=0;
        }

        public async Task<List<SavingsConfiguration>> GetSavingsConfiguration()
        {
            return await _dataContext.SavingsConfigurations.Where(a => a.StartDate <= DateTime.Now).ToListAsync();
        }
    }
}
