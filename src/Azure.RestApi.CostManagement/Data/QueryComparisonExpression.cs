namespace Azure.RestApi.CostManagement.Data;

using System.Runtime.Serialization;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#querycomparisonexpression
/// </remarks>
public sealed class QueryComparisonExpression
{
    public string? name { get; set; }


    [DataMember(Name = "operator")]
    public QueryOperatorType operatorType { get; set; }

    public string[]? values { get; set; }
}