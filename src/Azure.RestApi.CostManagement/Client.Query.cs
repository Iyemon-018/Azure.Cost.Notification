﻿namespace Azure.RestApi.CostManagement;

using System.Net;
using Data;
using Requests;

public partial class Client : IQuery
{
    async Task<AzureResponse<QueryResult>> IQuery.UsageAsync(QueryScope            scope
                                                           , QueryUsageRequestBody body
                                                           , string                skipToken         = default!
                                                           , string?               apiVersion        = default
                                                           , CancellationToken     cancellationToken = default)
    {
        var uri      = $"https://management.azure.com/{scope}/providers/Microsoft.CostManagement/query?api-version={apiVersion ?? Constants.LatestVersion}{(string.IsNullOrEmpty(skipToken) ? $"$skiptoken={skipToken}": string.Empty)}";
        var request  = RestApiRequest<QueryUsageRequestBody>.AsStringContent(uri, body);
        var response = await _tokenClient.PostAsync(request, cancellationToken).ConfigureAwait(false);

        return await CreateResponseAsync<QueryResult>(response, HttpStatusCode.OK, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}