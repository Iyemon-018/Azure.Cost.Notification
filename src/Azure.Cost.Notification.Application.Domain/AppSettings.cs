namespace Azure.Cost.Notification.Application.Domain;

using System.Text.Json.Serialization;

public sealed class AppSettings
{
    [JsonPropertyName("tenant_id")]
    public string TenantId { get; set; }

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }

    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; }

    [JsonPropertyName("subscription_id")]
    public string SubscriptionId { get; set; }
}