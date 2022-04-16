namespace Azure.RestApi.CostManagement;

using System.Net;
using System.Text.Json;
using Data;

public sealed partial class Client : IClient
{
    private readonly TokenClient _tokenClient;

    public Client(HttpClient httpClient)
    {
        _tokenClient = new TokenClient(httpClient);
    }

    public ILogin Login => this;

    public IQuery Query => this;

    private async Task<AzureResponse<T>> CreateResponseAsync<T>(RestApiResponse        response
                                                              , HttpStatusCode         successCode
                                                              , JsonSerializerOptions? options           = default
                                                              , CancellationToken      cancellationToken = default)
            where T : class
    {
        if (response.StatusCode == successCode)
        {
            var content = await response.DeserializeAsync<T>(options: options, cancellationToken: cancellationToken).ConfigureAwait(false);
            return new AzureResponse<T>(content, response.StatusCode, response.RequestMessage);
        }
        else
        {
            var error = await response.DeserializeAsync<ErrorResponse>(options: options, cancellationToken: cancellationToken).ConfigureAwait(false);
            return new AzureResponse<T>(response.StatusCode, response.RequestMessage, error);
        }
    }
}