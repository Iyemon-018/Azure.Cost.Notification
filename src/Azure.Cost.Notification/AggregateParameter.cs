namespace Azure.Cost.Notification;

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Application.Domain.Models;
using Domain.ValueObjects;
using Exceptions;

public sealed class AggregateParameter
{
    public AggregateParameter(HttpRequestMessage req)
    {
        // パラメータを取り出したい。
        // req.RequestUri.Query で取得できるのは"?～"なので、.Query[1..] して"?"だけ取り出している。
        // parameter=value のような形式を key:parameter, value:value という連想配列を作ることで
        // この後でパラメータの取り出しやすく、検証もしやすくしている。
        var parameters = req.RequestUri.Query.Skip(1).Any()
                ? req.RequestUri?
                     .Query[1..]
                     .Split('&')
                     .Select(x => x.Split('='))
                     .ToDictionary(x => x[0], x => x[1])
                : new Dictionary<string, string>();

        VerifyParameters(parameters);

        RoomId           = new RoomId(parameters);
        ChatworkApiToken = new ChatworkApiToken(parameters);
        SubscriptionId   = new SubscriptionId(parameters);
        TenantId         = new TenantId(parameters);
        ClientId         = new ClientId(parameters);
        ClientSecret     = new ClientSecret(parameters);
    }

    private void VerifyParameters(Dictionary<string, string> parameters)
    {
        var names = new[] {"roomId", "chatworkApiToken", "subscriptionId", "tenantId", "clientId", "clientSecret"};
        foreach (var name in names)
        {
            if (!parameters.ContainsKey(name)) throw new ParameterNotExistException(name);
        }
    }

    public RoomId RoomId { get; }

    public ChatworkApiToken ChatworkApiToken { get; }

    public SubscriptionId SubscriptionId { get; }

    public TenantId TenantId { get; }

    public ClientId ClientId { get; }

    public ClientSecret ClientSecret { get; }

    public AzureAccessTokenRequest AsAzureAccessTokenRequest() => new(TenantId.Value, ClientId.Value, ClientSecret.Value);
}