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
            var totalCost  = costResult.TotalCost();
            var highAmount = costResult.AsResourcesCost();

            var builder = new MessageBuilder();

            builder.Information.Add(title, $"{totalCost:C2}{Environment.NewLine}{highAmount}");

            yield return new ChatworkMessage(builder.Build());
        }
    }
}