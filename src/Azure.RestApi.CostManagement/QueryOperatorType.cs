namespace Azure.RestApi.CostManagement;

using System.Runtime.Serialization;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#queryoperatortype
/// </remarks>
public enum QueryOperatorType
{
    [EnumMember(Value = nameof(In))]
    In,
}