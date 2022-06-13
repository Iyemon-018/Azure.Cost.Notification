namespace Azure.Cost.Notification.Tests.Infrastructure.RestApi.Repositories;

using System;
using System.Net;
using System.Threading;
using Azure.RestApi.CostManagement;
using Azure.RestApi.CostManagement.Data;
using Azure.RestApi.CostManagement.Requests;
using Moq;

public static class QueryMockExtensions
{
    public static void UsageAsyncMock(this Mock<IQuery> self, Func<QueryScope, QueryUsageRequestBody, string, string, CancellationToken, AzureResponse<QueryResult>> func)
        => self.Setup(x => x.UsageAsync(It.IsAny<QueryScope>()
                      , It.IsAny<QueryUsageRequestBody>()
                      , It.IsAny<string>()
                      , It.IsAny<string>()
                      , It.IsAny<CancellationToken>()))
               .ReturnsAsync(func);

    public static void UsageAsyncErrorMock(this Mock<IQuery> self)
        => self.UsageAsyncMock((scope, body, skipToken, apiVersion, token) => AzureResponseBuilder.Error<QueryResult>(HttpStatusCode.Unauthorized));
}