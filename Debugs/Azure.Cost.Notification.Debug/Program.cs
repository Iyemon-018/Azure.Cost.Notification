// See https://aka.ms/new-console-template for more information

using Azure.Cost.Notification.Application.Domain;
using Azure.Cost.Notification.Application.Domain.Models;
using Azure.Cost.Notification.Domain.ValueObjects;
using Azure.Cost.Notification.Infrastructure.RestApi;
using System.Text.Json;

Console.WriteLine("Hello, World!");

await using var stream = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "appsettings.json"));
var appSettings = await JsonSerializer.DeserializeAsync<AppSettings>(stream).ConfigureAwait(false);

var unitOfWork = Factories.UnitOfWork(new HttpClient());
var accessToken = await unitOfWork.LoginRepository.Authenticate(tenantId: appSettings.TenantId
                                         , clientId: appSettings.ClientId
                                         , clientSecret: appSettings.ClientSecret)
                                  .ConfigureAwait(false);

Console.WriteLine($"----- Daily cost ----");
{
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

    Console.WriteLine($"[{dailyCost.Target:yyyy-MM-dd}]");
    foreach (var line in lines) Console.WriteLine(line);
}

Console.WriteLine($"----- Weekly cost ----");
{
    WeeklyCost weeklyCost;
    try
    {
        weeklyCost = await unitOfWork.ResourceUsageRepository
                                     .GetWeeklyCostAsync(appSettings.SubscriptionId, DateTime.UtcNow.Date.AddDays(-1))
                                     .ConfigureAwait(false);
    }
    catch (AzureRestApiException e)
    {
        Console.WriteLine(e);
        return;
    }
    
    var lines = new TotalCostResult(weeklyCost).TakeHighAmount(5)
                                           .Select(x => $"{x.ResourceGroupName} - {x.Id}({x.ServiceName}) | {x.Cost} JPY.")
                                           .ToArray();

    Console.WriteLine($"[{weeklyCost.PeriodFrom:yyyy-MM-dd} - {weeklyCost.PeriodTo:yyyy-MM-dd}]");
    foreach (var line in lines) Console.WriteLine(line);
}

Console.WriteLine($"----- Monthly cost ----");
{
    MonthlyCost monthlyCost;
    try
    {
        monthlyCost = await unitOfWork.ResourceUsageRepository
                                      .GetMonthlyCostAsync(appSettings.SubscriptionId)
                                      .ConfigureAwait(false);
    }
    catch (AzureRestApiException e)
    {
        Console.WriteLine(e);
        return;
    }

    var lines = new TotalCostResult(monthlyCost).TakeHighAmount(5)
                                            .Select(x => $"{x.ResourceGroupName} - {x.Id}({x.ServiceName}) | {x.Cost} JPY.")
                                            .ToArray();

    foreach (var line in lines) Console.WriteLine(line);
}