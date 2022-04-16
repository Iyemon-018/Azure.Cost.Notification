namespace Azure.RestApi.CostManagement;

using Data;

internal sealed class TokenClient
{
    private readonly HttpClient _httpClient;

    public TokenClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RestApiResponse> PostAsync<T>(RestApiRequest<T> request, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.PostAsync(request.Uri, request.Content, cancellationToken).ConfigureAwait(false);
        return new RestApiResponse(response);
    }

    public void AccessToken(AccessToken token) => _httpClient.DefaultRequestHeaders.Authorization = token.AsHeaderValue();
}