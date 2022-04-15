namespace Azure.RestApi.CostManagement;

using System.Runtime.Serialization;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#functiontype
/// </remarks>
public enum FunctionType
{
    [EnumMember(Value = nameof(Sum))]
    Sum,
}