namespace Azure.Cost.Notification.Application.Domain;

using System.Text.Json.Serialization;

public sealed class AppSettings
{
    [JsonPropertyName("tenant_id")]
    public string TenantId { get; set; } = null!;

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; } = null!;

    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; } = null!;

    [JsonPropertyName("subscription_id")]
    public string SubscriptionId { get; set; } = null!;
}