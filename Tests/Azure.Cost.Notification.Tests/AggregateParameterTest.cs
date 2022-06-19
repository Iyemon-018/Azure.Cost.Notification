namespace Azure.Cost.Notification.Tests;

using System.Net.Http;
using ChainingAssertion;
using Exceptions;
using Xunit;

public class AggregateParameterTest
{
    [Fact]
    public void Test_ctor_各パラメータがすべて設定されている場合にすべての値が取得できること()
    {
        var req    = new HttpRequestMessage(HttpMethod.Get, "https://azure.website/api/cost/aggregate?roomId=1082&chatworkApiToken=accedkaks10&subscriptionId=1234-5678-90abc-defg&tenantId=223940dsa0s&clientId=209931&clientSecret=Hlh5divW.Z0.CoPpwkGsjzIY5fcxRvQD7W");

        var target = new AggregateParameter(req);
        
        target.RoomId.Value.Is(1082);
        target.ChatworkApiToken.Value.Is("accedkaks10");
        target.SubscriptionId.Value.Is("1234-5678-90abc-defg");
        target.TenantId.Value.Is("223940dsa0s");
        target.ClientId.Value.Is("209931");
        target.ClientSecret.Value.Is("Hlh5divW.Z0.CoPpwkGsjzIY5fcxRvQD7W");
    }

    [Theory]
    [InlineData("https://azure.website/api/cost/aggregate", "roomId")]
    [InlineData("https://azure.website/api/cost/aggregate?roomId=1082", "chatworkApiToken")]
    [InlineData("https://azure.website/api/cost/aggregate?roomId=1082&chatworkApiToken=accedkaks10", "subscriptionId")]
    [InlineData("https://azure.website/api/cost/aggregate?roomId=1082&chatworkApiToken=accedkaks10&subscriptionId=1234-5678-90abc-defg", "tenantId")]
    [InlineData("https://azure.website/api/cost/aggregate?roomId=1082&chatworkApiToken=accedkaks10&subscriptionId=1234-5678-90abc-defg&tenantId=223940dsa0s", "clientId")]
    [InlineData("https://azure.website/api/cost/aggregate?roomId=1082&chatworkApiToken=accedkaks10&subscriptionId=1234-5678-90abc-defg&tenantId=223940dsa0s&clientId=209931", "clientSecret")]
    [InlineData("https://azure.website/api/cost/aggregate?chatworkApiToken=accedkaks10&subscriptionId=1234-5678-90abc-defg&tenantId=223940dsa0s&clientId=209931&clientSecret=Hlh5divW.Z0.CoPpwkGsjzIY5fcxRvQD7W", "roomId")]
    [InlineData("https://azure.website/api/cost/aggregate?roomId=1082&subscriptionId=1234-5678-90abc-defg&tenantId=223940dsa0s&clientId=209931&clientSecret=Hlh5divW.Z0.CoPpwkGsjzIY5fcxRvQD7W", "chatworkApiToken")]
    [InlineData("https://azure.website/api/cost/aggregate?roomId=1082&chatworkApiToken=accedkaks10&tenantId=223940dsa0s&clientId=209931&clientSecret=Hlh5divW.Z0.CoPpwkGsjzIY5fcxRvQD7W", "subscriptionId")]
    [InlineData("https://azure.website/api/cost/aggregate?roomId=1082&chatworkApiToken=accedkaks10&subscriptionId=1234-5678-90abc-defg&clientId=209931&clientSecret=Hlh5divW.Z0.CoPpwkGsjzIY5fcxRvQD7W", "tenantId")]
    [InlineData("https://azure.website/api/cost/aggregate?roomId=1082&chatworkApiToken=accedkaks10&subscriptionId=1234-5678-90abc-defg&tenantId=223940dsa0s&clientSecret=Hlh5divW.Z0.CoPpwkGsjzIY5fcxRvQD7W", "clientId")]
    public void Test_ctor_それぞれのパラメータが設定されていない場合に例外がスローされること(string url, string name)
    {
        var req = new HttpRequestMessage(HttpMethod.Get, url);

        var ex = Record.Exception(() => new AggregateParameter(req));

        ex.IsInstanceOf<ParameterNotExistException>();
        (ex as ParameterNotExistException)!.Message.Is($"パラメータ[{name}]が指定されていません。[{name}]に値を設定してください。");
    }
}