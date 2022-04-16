namespace Azure.RestApi.CostManagement.Data;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#errorresponse
/// </remarks>
public sealed class ErrorResponse
{
    public string code { get; set; }

    public string message { get; set; }
    
    public override string ToString() => $"{code}: {message}";
}