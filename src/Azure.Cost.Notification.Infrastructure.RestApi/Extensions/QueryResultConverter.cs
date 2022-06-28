namespace Azure.Cost.Notification.Infrastructure.RestApi.Extensions;

using System.Text.Json;
using Azure.RestApi.CostManagement.Data;
using Domain.Entities;
using Models;

internal static class QueryResultConverter
{
    public static IEnumerable<ResourceUsage> AsResourceUsages(this QueryResult self)
    {
        var position = new CostManagementQueryColumnPosition(self.properties.columns);
        return self.properties.rows.Select(x => x.AsResourceUsage(position));
    }

    internal static ResourceUsage AsResourceUsage(this JsonElement[] self, CostManagementQueryColumnPosition position)
        => new(cost: self[position.PreTaxCostIndex].GetDecimal()
              , resourceGroupName: self[position.ResourceGroupNameIndex].GetString() ?? string.Empty
              , serviceName: self[position.ServiceNameIndex].GetString() ?? string.Empty
              , id: (self[position.ResourceIdIndex].GetString() ?? string.Empty).Split('/').Last());
}