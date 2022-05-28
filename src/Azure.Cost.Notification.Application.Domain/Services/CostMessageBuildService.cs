namespace Azure.Cost.Notification.Application.Domain.Services;

using ChatworkApi.Messages;
using Models;

public sealed class CostMessageBuildService : ICostMessageBuildService
{
    public IEnumerable<ChatworkMessage> Build(TotalCostResult[] totalCosts)
    {
        foreach (var costResult in totalCosts)
        {
            var title      = costResult.AsTitle();
            var totalCost  = costResult.TotalCostJapaneseCurrency();
            var highAmount = costResult.AsResourcesCost();

            var builder = new MessageBuilder();

            builder.Information.Add(title, $"合計: {totalCost}{Environment.NewLine}[hr]{Environment.NewLine}利用料の高いリソース{Environment.NewLine}{highAmount}");

            yield return new ChatworkMessage(builder.Build());
        }
    }
}