namespace Azure.RestApi.CostManagement.Data;

using System.Text.Json;

public sealed class QueryResultProperty
{
    public string nextLink { get; set; } = null!;

    public QueryColumn[] columns { get; set; } = null!;

    public JsonElement[][] rows { get; set; } = null!;

    /// <summary>
    /// <seealso cref="nextLink"/> が定義されている場合に skiptoken の値を取得します。
    /// </summary>
    /// <returns>skiptoken の値を返します。<seealso cref="nextLink"/> が定義されていない場合は <see cref="string.Empty"/> を返します。</returns>
    public string SkipToken()
    {
        const string skipToken = "$skiptoken";

        if (!nextLink.Contains(skipToken)) return string.Empty;

        var index = nextLink.IndexOf(skipToken, StringComparison.Ordinal);
        return nextLink[index..].Split('=').Last();
    }

    public bool HasNext => !string.IsNullOrEmpty(nextLink);
}