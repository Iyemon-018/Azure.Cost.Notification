// See https://aka.ms/new-console-template for more information

using Azure.RestApi.CostManagement;
using Azure.RestApi.CostManagement.Data;
using Azure.RestApi.CostManagement.Requests;
using System.Text.Json;
using Azure.Cost.Notification.Application.Domain;

Console.WriteLine("Hello, World!");

await using var stream = File.OpenRead(Path.Combine(Environment.CurrentDirectory, "appsettings.json"));
var appSettings = await JsonSerializer.DeserializeAsync<AppSettings>(stream);

var client      = new Client(new HttpClient());
var requestBody = AccessTokenRequestBody.AsClientCredentials(appSettings.ClientId, appSettings.ClientSecret);
var accessToken = await client.Login.GetAccessTokenAsync(appSettings.TenantId, requestBody).ConfigureAwait(false);

client.AccessToken(accessToken.Content);

var body = new QueryUsageRequestBody
           {
               type          = ExportType.ActualCost
             , timeframeType = TimeframeType.BillingMonthToDate
             , dataset = new QueryDataset
                         {
                             aggregation = QueryAggregationDictionary.Default()
                           , granularity = GranularityType.Monthly
                           , grouping = new[]
                                        {
                                            QueryGrouping.ResourceGroupName()
                                          , QueryGrouping.ServiceName()
                                          , QueryGrouping.ResourceId()
                                        }
                         }
           };
var response = await client.Query
                           .UsageAsync(QueryScope.Subscriptions(appSettings.SubscriptionId), body)
                           .ConfigureAwait(false);

Console.WriteLine(response.Content.ToString());
