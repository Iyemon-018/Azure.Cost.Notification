namespace Azure.Cost.Notification.Infrastructure.RestApi.Extensions;

using System.Text.Json;
using Azure.RestApi.CostManagement.Data;
using Domain.ValueObjects;
using Models;

internal static class QueryResultConverter
{
    public static DailyCost AsDailyCost(this QueryResult self, DateTime target)
    {
        var position = new CostManagementQueryColumnPosition(self.properties.columns);
        var usage    = self.properties.rows.Select(x => x.ToDailyUsage(position)).ToArray();

        return new DailyCost(target, usage);
    }

    internal static ResourceUsage ToDailyUsage(this JsonElement[] self, CostManagementQueryColumnPosition position)
    {
        // ToDailyUsage ってしてるけど、Weekly も Monthly も区別する必要ないかも？
        // 区別する理由がそれぞれの種別ごとに日付のフォーマットが違ってたから。

        // ここがちょっと動くかどうか怪しいかも？
        return new ResourceUsage(cost: self[position.PreTaxCostIndex].GetDecimal()
              , resourceGroupName: self[position.ResourceGroupNameIndex].GetString()
              , serviceName: self[position.ServiceNameIndex].GetString()
              , id: self[position.ResourceIdIndex].GetString().Split('/').Last());
    }
}