namespace Azure.RestApi.CostManagement;

using System.Net;
using System.Text.Json;

internal sealed class RestApiResponse
{
    private readonly HttpResponseMessage _response;

    public RestApiResponse(HttpResponseMessage response)
    {
        _response      = response;
        StatusCode     = _response.StatusCode;
        RequestMessage = response.RequestMessage!;
    }

    public HttpStatusCode StatusCode { get; }

    public HttpRequestMessage RequestMessage { get; }

    public async Task<T> DeserializeAsync<T>(JsonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        await using var stream = await _response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);

        return (await JsonSerializer.DeserializeAsync<T>(stream
                                           , options ?? Constants.JsonSerializerOptions
                                           , cancellationToken)
                                    .ConfigureAwait(false))!;
    }
}