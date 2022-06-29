namespace Azure.Cost.Notification.Application.Domain.Services;

using Infrastructure.ChatworkApi;
using Models;
using Notification.Domain.Models;

public sealed class CostMessageBuildService : ICostMessageBuildService
{
    public IEnumerable<ChatworkMessage> Build(int roomId, TotalCostResult[] totalCosts)
    {
        foreach (var costResult in totalCosts)
        {
            var title      = costResult.AsTitle();
            var totalCost  = costResult.TotalCostJapaneseCurrency();
            var highAmount = costResult.AsResourcesCost();
            var message    = $"合計: {totalCost}{Environment.NewLine}[hr]利用料の高いリソース{Environment.NewLine}{highAmount}";

            yield return ChatworkMessageBuilder.Build(roomId, title, message);
        }
    }
}