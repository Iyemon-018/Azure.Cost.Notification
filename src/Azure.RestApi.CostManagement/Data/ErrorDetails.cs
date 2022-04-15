namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#errordetails
/// </remarks>
public sealed class ErrorDetails
{
    public string code { get; set; }

    public string message { get; set; }
}