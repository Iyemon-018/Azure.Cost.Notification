namespace Azure.Cost.Notification.Application.Domain.Models;

public sealed class AzureAccessTokenRequest
{
    public AzureAccessTokenRequest(string tenantId, string clientId, string clientSecret)
    {
        TenantId     = tenantId;
        ClientId     = clientId;
        ClientSecret = clientSecret;
    }

    public string TenantId { get; }

    public string ClientId { get; }

    public string ClientSecret { get; }
}