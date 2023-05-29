namespace WebApi.Helpers;

using Microsoft.EntityFrameworkCore;
using WebApi.Entities;

public class DataContext : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<SavingsAccount> SavingsAccounts { get; set; }
    public DbSet<SavingsConfiguration> SavingsConfigurations { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    private readonly IConfiguration Configuration;

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sqlite database
        options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
    }
}