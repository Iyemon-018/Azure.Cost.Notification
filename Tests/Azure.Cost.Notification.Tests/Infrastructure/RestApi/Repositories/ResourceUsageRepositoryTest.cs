namespace Azure.Cost.Notification.Tests.Infrastructure.RestApi.Repositories;

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Azure.RestApi.CostManagement.Data;
using Azure.RestApi.CostManagement.Requests;
using ChainingAssertion;
using Moq;
using Notification.Domain.Repositories;
using Notification.Domain.ValueObjects;
using Notification.Infrastructure.RestApi;
using Notification.Infrastructure.RestApi.Repositories;
using Xunit;

public class ResourceUsageRepositoryTest
{
    private static class Builder
    {
        public static QueryResult FromDailyCost(string fileName = "DailyCost.json")
            => JsonSerializer.Deserialize<QueryResult>(File.ReadAllText($"TestData\\{nameof(ResourceUsageRepositoryTest)}\\{fileName}"))!;
    }

    private readonly TestFactory _testFactory;

    private readonly IResourceUsageRepository _target;

    public ResourceUsageRepositoryTest()
    {
        _testFactory = new TestFactory();
        _target      = new ResourceUsageRepository(_testFactory.Client);
    }

    [Fact]
    public void Test_GetDailyCostAsync_APIエラーの場合にAzureRestApiExceptionがスローされること()
    {
        _testFactory.QueryMock.UsageAsyncMock((scope, body, skipToken, apiVersion, token)
                => AzureResponseBuilder.Error<QueryResult>(HttpStatusCode.BadGateway, requestUri: "https://www.google.co.jp/"));

        var ex = Record.ExceptionAsync(() => _target.GetDailyCostAsync("0000-1111-aaaa-bbbb", DateTime.Today));

        ex.Result.IsInstanceOf<AzureRestApiException>();

        var e = ex.Result as AzureRestApiException;
        e.StatusCode.Is(HttpStatusCode.BadGateway);
        e.Uri.AbsoluteUri.Is("https://www.google.co.jp/");
    }

    [Fact]
    public void Test_GetDailyCostAsync_引数の値が正確に引き渡されていることを確認する()
    {
        var date = DateTime.Today;

        _testFactory.QueryMock.UsageAsyncMock((scope, body, skipToken, apiVersion, token)
                =>
        {
            scope.ToString().Is("subscriptions/0000-1111-aaaa-bbbb");
            body.timePeriod.from.Is(date);
            body.timePeriod.to.Is(date);
            skipToken.Is(string.Empty);
            apiVersion.Is("2021-10-01");
            token.Is(CancellationToken.None);

            return AzureResponseBuilder.Success(Builder.FromDailyCost());
        });

        var cost = _target.GetDailyCostAsync("0000-1111-aaaa-bbbb", date).Result;
    }

    [Fact]
    public async Task Test_GetDailyCostAsync_応答が１回の場合に応答結果の値を確認する()
    {
        var targetDate = DateTime.Today;

        _testFactory.QueryMock.UsageAsyncMock((scope, body, skipToken, apiVersion, token)
                => AzureResponseBuilder.Success(Builder.FromDailyCost("DailyCost_Onece.json")));

        var cost = await _target.GetDailyCostAsync("0000-1111-aaaa-bbbb", targetDate).ConfigureAwait(false);

        cost.IsNotNull();
        cost.Target.Is(targetDate);
        // DailyCost_Onece.json の rows 参照
        cost.Usage.Count().Is(3);
        cost.Usage
            .Is(new ResourceUsage(0.000392m, "motex.develop.business", "API Management", "motex-develop-works-apim")
                   , new ResourceUsage(0.0m, "account-profile", "Azure App Service", "account-profile")
                   , new ResourceUsage(0.0m, "azure.devops.hooks", "Azure App Service", "pipelinenotifer"));
    }

    [Fact]
    public async Task Test_GetDailyCostAsync_応答が２回の場合に応答結果の値を確認する()
    {
        var targetDate = DateTime.Today;

        _testFactory.QueryMock
                    .Setup(x => x.UsageAsync(It.IsAny<QueryScope>()
                           , It.IsAny<QueryUsageRequestBody>()
                           , It.Is<string>(skipToken => string.IsNullOrEmpty(skipToken))
                           , It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => AzureResponseBuilder.Success(Builder.FromDailyCost("DailyCost_Twice_1.json")));
        _testFactory.QueryMock
                    .Setup(x => x.UsageAsync(It.IsAny<QueryScope>()
                           , It.IsAny<QueryUsageRequestBody>()
                           , It.Is<string>(skipToken => !string.IsNullOrEmpty(skipToken))
                           , It.IsAny<string>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => AzureResponseBuilder.Success(Builder.FromDailyCost("DailyCost_Twice_2.json")));

        var cost = await _target.GetDailyCostAsync("0000-1111-aaaa-bbbb", targetDate).ConfigureAwait(false);

        cost.IsNotNull();
        cost.Target.Is(targetDate);
        // DailyCost_Twice_1.json, DailyCost_Twice_2.json の rows 参照
        cost.Usage.Count().Is(5);
        cost.Usage.ElementAt(0).Is(new ResourceUsage(0.000392m, "motex.develop.business", "API Management", "motex-develop-works-apim"));
        cost.Usage.ElementAt(1).Is(new ResourceUsage(0.0m, "account-profile", "Azure App Service", "account-profile"));
        cost.Usage.ElementAt(2).Is(new ResourceUsage(0.0m, "azure.devops.hooks", "Azure App Service", "pipelinenotifer"));
        cost.Usage.ElementAt(3).Is(new ResourceUsage(21.983625215999989m, "test-lab3225414888002", "Storage", "v9400-update-19-osdisk"));
        cost.Usage.ElementAt(4).Is(new ResourceUsage(21.983625215999989m, "test-lab4008206580000", "Storage", "win2019-sql2017-osdisk"));
    }
}