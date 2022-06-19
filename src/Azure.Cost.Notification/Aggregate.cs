namespace Azure.Cost.Notification;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Domain.Models;
using Domain.Extensions;
using Domain.ValueObjects;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

public sealed class Aggregate
{
    [FunctionName($"{nameof(Aggregate)}_{nameof(Orchestrator)}")]
    public async Task<string> Orchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context
            , ILogger log)
    {
        var messages       = Enumerable.Empty<ChatworkMessage>();
        var sendResult     = Enumerable.Empty<ChatworkSendResult>();

        var parameter      = context.GetInput<AggregateParameter>();

        var roomId         = parameter.RoomId.Value;
        var subscriptionId = parameter.SubscriptionId.Value;

        try
        {
            // アクセストークンを取得する。
            var tokenRequest = parameter.AsAzureAccessTokenRequest();
            var token        = await context.CallActivityAsync<AzureAuthentication>($"{nameof(SharedActivity)}_{nameof(SharedActivity.GetAccessToken)}", tokenRequest);

            // 取得したアクセストークンを使用して、CostManagement API を呼び出す。(3つ)
            var collectTasks = new[]
                               {
                                   context.CallActivityAsync<TotalCostResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.DailyTotalCost)}", subscriptionId)
                                 , context.CallActivityAsync<TotalCostResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.WeeklyTotalCost)}", subscriptionId)
                                 , context.CallActivityAsync<TotalCostResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.MonthlyTotalCost)}", subscriptionId)
                               };
            var totalCostResults = await Task.WhenAll(collectTasks);

            // 送信用のメッセージ形式にフォーマットする。
            messages = await context.CallActivityAsync<IEnumerable<ChatworkMessage>>($"{nameof(SharedActivity)}_{nameof(SharedActivity.FormatChatworkMessage)}", (roomId, totalCostResults));
        }
        catch (Exception e)
        {
            // 失敗した場合でもチャットに失敗したことの通知は出したい。
            // でなければ成功したのか、実行されていないのか判断できないので。
            messages = new[] {new ChatworkMessage(roomId, "Azure 利用料金の通知に失敗しました。")};
            log.LogError(e, $"Failed aggregate azure cost.[{messages}]");
        }
        finally
        {
            // チャットに結果をまとめて送信する。
            sendResult = await context.CallActivityAsync<IEnumerable<ChatworkSendResult>>($"{nameof(SharedActivity)}_{nameof(SharedActivity.SendChatwork)}", messages);
        }

        return sendResult.Logs();
    }

    [FunctionName($"{nameof(Aggregate)}_{nameof(HttpStart)}")]
    public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "api/cost/aggregate")]
            HttpRequestMessage req
          , [DurableClient] IDurableOrchestrationClient starter
          , ILogger                                     log)
    {
        var parameter  = new AggregateParameter(req);
        var instanceId = await starter.StartNewAsync($"{nameof(Aggregate)}_{nameof(Orchestrator)}", parameter);

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}