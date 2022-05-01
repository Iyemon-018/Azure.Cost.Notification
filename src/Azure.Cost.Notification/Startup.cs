using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Azure.Cost.Notification.Startup))]
namespace Azure.Cost.Notification;

using System.Net.Http;
using Application.Domain.Services;
using Infrastructure.RestApi;
using Microsoft.Extensions.DependencyInjection;

public sealed class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddHttpClient();

        builder.Services.AddSingleton<IUnitOfWork>(x => Factories.UnitOfWork(x.GetService<HttpClient>()));
        builder.Services.AddSingleton<IAccessTokenRequestService, AccessTokenRequestService>();
    }
}