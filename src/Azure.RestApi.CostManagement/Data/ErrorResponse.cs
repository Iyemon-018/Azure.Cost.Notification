namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#errorresponse
/// </remarks>
public sealed class ErrorResponse
{
    public ErrorDetails error { get; set; }
}