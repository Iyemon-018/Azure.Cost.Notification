// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using Azure.Cost.Notification.Application.Domain;
using Azure.Cost.Notification.Application.Domain.Models;
using Azure.Cost.Notification.Domain.ValueObjects;
using Azure.Cost.Notification.Infrastructure.RestApi;

Console.WriteLine("Hello, World!");

await using var stream      = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "appsettings.json"));
var             appSettings = await JsonSerializer.DeserializeAsync<AppSettings>(stream).ConfigureAwait(false);

var unitOfWork = Factories.UnitOfWork(new HttpClient());
var accessToken = await unitOfWork.LoginRepository.Authenticate(tenantId: appSettings.TenantId
                                         , clientId: appSettings.ClientId
                                         , clientSecret: appSettings.ClientSecret)
                                  .ConfigureAwait(false);

DailyCost dailyCost;
try
{
    // 日付の指定は UTC のほうが正確。
    dailyCost = await unitOfWork.ResourceUsageRepository
                                .GetDailyCostAsync(appSettings.SubscriptionId, DateTime.UtcNow.Date.AddDays(-1))
                                .ConfigureAwait(false);
}
catch (AzureRestApiException e)
{
    Console.WriteLine(e.Message);
    return;
}

var lines = new TotalCostResult(dailyCost).TakeHighAmount(5)
                                          .Select(x => $"{x.ResourceGroupName} - {x.Id}({x.ServiceName}) | {x.Cost} JPY.")
                                          .ToArray();

foreach (var line in lines)
{
    Console.WriteLine(line);
}
