namespace Azure.Cost.Notification.Infrastructure.RestApi.Models;

using Azure.RestApi.CostManagement.Data;

/// <summary>
/// Azure Cost Management - Query REST API によって取得したデータの列の位置を取得するためのクラスです。
/// </summary>
/// <remarks>
/// "properties" - "rows" に欲しい情報が定義されているが、ジャグ配列になっていてどの位置に何があるかを把握しておく必要がある。
/// "properties" - "columns" にその位置の情報が記載されているので、それぞれの列のインデックスをこのクラスで保持する。
/// そのため、アプリケーションで使用したい情報を REST API で取得したくなった場合は、このクラスで対象の列のインデックスを新たに取得する必要がある。
/// </remarks>
internal sealed class CostManagementQueryColumnPosition
{
    public CostManagementQueryColumnPosition(QueryColumn[] columns)
    {
        var columnsWithIndex = columns.Select((x, i) => new {column = x, index = i}).ToArray();

        PreTaxCostIndex        = columnsWithIndex.First(x => x.column.name == "PreTaxCost").index;
        ResourceGroupNameIndex = columnsWithIndex.First(x => x.column.name == "ResourceGroupName").index;
        ServiceNameIndex       = columnsWithIndex.First(x => x.column.name == "ServiceName").index;
        ResourceIdIndex        = columnsWithIndex.First(x => x.column.name == "ResourceId").index;
    }

    public int PreTaxCostIndex { get; }

    public int ResourceGroupNameIndex { get; }

    public int ServiceNameIndex { get; }

    public int ResourceIdIndex { get; }
}