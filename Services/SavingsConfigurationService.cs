using WebApi.Entities;
using WebApi.Enums;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class SavingsConfigurationService : ISavingsConfigurationService
    {
        private readonly ISavingsConfigurationRepository _savingsConfigurationRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ISavingsAccountRepository _savingsAccountRepository;

        public SavingsConfigurationService(ISavingsConfigurationRepository savingsConfigurationRepository, ITransactionRepository transactionRepository, ISavingsAccountRepository savingsAccountRepository)
        {
            _savingsConfigurationRepository = savingsConfigurationRepository;
            _transactionRepository = transactionRepository;
            _savingsAccountRepository = savingsAccountRepository;
        }

        public async Task<bool> CreateSavingsConfiguration(SavingsConfiguration savingsConfiguration)
        {
            return await _savingsConfigurationRepository.CreateSavingsConfiguration(savingsConfiguration);
        }

        public async Task<List<SavingsConfiguration>> GetSavingsConfiguration()
        {
            return await _savingsConfigurationRepository.GetSavingsConfiguration();
        }

        public async Task ProcessSavings()
        {
            var savingsConfigurations =  await GetSavingsConfiguration();
            var today = DateTime.Today;
            var expenses = new List<Transaction>();
            foreach (var savingsConfiguration in savingsConfigurations)
            {
                var timeInterval = (today - savingsConfiguration.StartDate.Date).TotalDays;
                if (savingsConfiguration.PeriodType == Enums.PeriodType.Daily)
                {
                    expenses = await _transactionRepository.GetSpending(savingsConfiguration.BankAccountId, today.AddDays(-1), today);
                    await ProcessSavings(savingsConfiguration, expenses);
                }
                else if (savingsConfiguration.PeriodType == Enums.PeriodType.Weekly && timeInterval % 7 == 0)
                {
                    expenses = await _transactionRepository.GetSpending(savingsConfiguration.BankAccountId, today.AddDays(-7), today);
                    await ProcessSavings(savingsConfiguration, expenses);

                }
                else if (savingsConfiguration.PeriodType == Enums.PeriodType.Monthly && timeInterval % 30 == 0)
                {
                    expenses = await _transactionRepository.GetSpending(savingsConfiguration.BankAccountId, today.AddDays(-30), today);
                    await ProcessSavings(savingsConfiguration, expenses);

                }
            }
        }

        private async Task ProcessSavings(SavingsConfiguration savingsConfiguration, List<Transaction> expenses)
        {
            double amountToBeSaved = 0;
            var bankAccount = expenses.FirstOrDefault().BankAccount;
            switch (savingsConfiguration.SavingsType)
            {
                case Enums.SavingsType.Fixed:
                    amountToBeSaved = savingsConfiguration.FixedSaving;
                    break;

                case Enums.SavingsType.SpendingPercentage:
                    var spent = expenses.Sum(x => x.Amount);
                    amountToBeSaved = spent * savingsConfiguration.Percentage;
                    break;

                case Enums.SavingsType.BalancePercentage:
                    amountToBeSaved = bankAccount.Balance * savingsConfiguration.Percentage;
                    break;
            }
            var transaction = new Transaction(bankAccount.Id, amountToBeSaved, TransactionType.Debit);
            await _transactionRepository.CreateTransactionAsync(transaction);
            await _savingsAccountRepository.CreditSavingsAccountAsync(savingsConfiguration.SavingsAccountId, amountToBeSaved);
        }
    }
}
