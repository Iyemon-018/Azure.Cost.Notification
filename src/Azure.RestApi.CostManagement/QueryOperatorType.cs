namespace Azure.RestApi.CostManagement;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#queryoperatortype
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QueryOperatorType
{
    [EnumMember(Value = nameof(In))]
    In,
}