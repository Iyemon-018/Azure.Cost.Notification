namespace Azure.RestApi.CostManagement;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#granularitytype
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GranularityType
{
    [EnumMember(Value = nameof(Daily))]
    Daily,
    [EnumMember(Value = nameof(Weekly))]
    Weekly,
    [EnumMember(Value = nameof(Monthly))]
    Monthly,
}