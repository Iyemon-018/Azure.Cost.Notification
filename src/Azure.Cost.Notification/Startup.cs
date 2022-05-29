using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Azure.Cost.Notification.Startup))]
namespace Azure.Cost.Notification;

using System.Globalization;
using System.Net.Http;
using Application.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

public sealed class Startup : FunctionsStartup
{
    public Startup()
    {
        CultureInfo.CurrentCulture = CultureInfo.CreateSpecificCulture("ja-JP");
    }

    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<Infrastructure.RestApi.IUnitOfWork>(x => Infrastructure.RestApi.Factories.UnitOfWork(x.GetService<HttpClient>()));
        builder.Services.AddSingleton<Infrastructure.ChatworkApi.IUnitOfWork>(x => Infrastructure.ChatworkApi.Factories.UnitOfWork(x.GetService<HttpClient>()));
        builder.Services.AddSingleton<IAccessTokenRequestService, AccessTokenRequestService>();
        builder.Services.AddSingleton<IUsageCostRequestService, UsageCostRequestService>();
        builder.Services.AddSingleton<ICostMessageBuildService, CostMessageBuildService>();
        builder.Services.AddSingleton<ISendMessageService, SendMessageService>();
    }
}