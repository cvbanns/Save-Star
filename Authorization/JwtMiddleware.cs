namespace WebApi.Authorization;

using Microsoft.Extensions.Options;
using WebApi.Helpers;
using WebApi.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;
    private readonly IAccountService _accountService;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings, IAccountService accountService)
    {
        _next = next;
        _appSettings = appSettings.Value;
        _accountService = accountService;
    }

    public async Task Invoke(HttpContext context, DataContext dataContext, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var accountId = jwtUtils.ValidateJwtToken(token);
        if (accountId != null)
        {
            // attach account to context on successful jwt validation
            _accountService.Account = await dataContext.Accounts.FindAsync(accountId.Value);
            context.Items["Account"] = _accountService.Account;
            //context.Items["Account"] = await dataContext.Accounts.FindAsync(accountId.Value);
        }

        await _next(context);
    }
}