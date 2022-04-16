namespace Azure.RestApi.CostManagement.Requests;

using System.Runtime.Serialization;

public sealed class AccessTokenRequestBody
{
    [DataMember(Name = "grant_type")]
    public GrantType grantType { get; set; }

    [DataMember(Name = "client_id")]
    public string clientId { get; set; }

    [DataMember(Name = "client_secret")]
    public string clientSecret { get; set; }

    [DataMember(Name = "resource")]
    public string resource { get; set; }

    public static AccessTokenRequestBody AsClientCredentials(string clientId, string clientSecret)
        => new()
           {
               grantType    = GrantType.ClientCredentials
             , resource     = @"https://management.core.windows.net/"
             , clientId     = clientId
             , clientSecret = clientSecret
           };
}