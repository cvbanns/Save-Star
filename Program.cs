using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WebApi.Authorization;
using WebApi.Helpers;
using WebApi.Repositories;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// add services to DI container
{
    var services = builder.Services;
    var env = builder.Environment;

    services.AddDbContext<DataContext>();
    services.AddCors();
    services.AddControllers().AddJsonOptions(x =>
    {
        // serialize enums as strings in api responses (e.g. Role)
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Scaffold API",
            Version = "v1"
        });
    });

    // configure strongly typed settings object
    services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

    // configure DI for application services
    services.AddScoped<IAccountService, AccountService>();
    services.AddTransient<IJwtUtils, JwtUtils>();
    services.AddTransient<IEmailService, EmailService>();
    services.AddTransient<ITransactionService, TransactionService>();
    services.AddTransient<ISavingsAccountService, SavingsAccountService>();
    services.AddTransient<ISavingsConfigurationService, SavingsConfigurationService>();
    services.AddTransient<ITransactionRepository, TransactionRepository>();
    services.AddTransient<ISavingsAccountRepository, SavingsAccountRepository>();
    services.AddTransient<ISavingsConfigurationRepository, SavingsConfigurationRepository>();
    services.AddHostedService<SavingsBackgroundService>();
}

var app = builder.Build();

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    using (var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>())
    {
        dataContext.Database.Migrate();
    }
}

// configure HTTP request pipeline
{
    //using (var accountServices = app.Services.CreateScope())
    //{
    //   var service = accountServices.ServiceProvider.GetService<IAccountService>();
    //   //string guid = service.GetDetails
    //}
    // generated swagger json and swagger ui middleware
    app.UseSwagger();
    app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", ".NET Sign-up and Verification API"));

    // global cors policy
    app.UseCors(x => x
        .SetIsOriginAllowed(origin => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

    // global error handler
    app.UseMiddleware<ErrorHandlerMiddleware>();

    // custom jwt auth middleware
    app.UseMiddleware<JwtMiddleware>();

    app.MapControllers();
}

app.Run("http://localhost:4000");