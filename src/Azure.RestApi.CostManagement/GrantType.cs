namespace Azure.RestApi.CostManagement;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GrantType
{
    [EnumMember(Value = "client_credentials")]
    ClientCredentials,
}