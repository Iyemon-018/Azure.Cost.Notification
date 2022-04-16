namespace Azure.RestApi.CostManagement;

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

/// <summary>
/// 
/// </summary>
/// <remarks>
/// cf. https://docs.microsoft.com/ja-jp/rest/api/cost-management/query/usage#querycolumntype
/// </remarks>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QueryColumnType
{
    [EnumMember(Value = nameof(Dimension))]
    Dimension,

    [EnumMember(Value = nameof(Tag))]
    Tag,
}