namespace Azure.RestApi.CostManagement;

using System.Runtime.Serialization;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#granularitytype
/// </remarks>
public enum GranularityType
{
    [EnumMember(Value = nameof(Daily))]
    Daily,
}