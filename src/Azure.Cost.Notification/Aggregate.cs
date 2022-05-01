namespace Azure.Cost.Notification;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Domain.Models;
using Domain.ValueObjects;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Models;

public sealed class Aggregate
{
    [FunctionName($"{nameof(Aggregate)}_{nameof(Orchestrator)}")]
    public async Task<string> Orchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context
            , ILogger log)
    {
        ChatworkMessage    chatworkMessage = null;
        ChatworkSendResult chatworkSendResult;

        try
        {
            // TODO アクセストークンを取得する。
            var tokenRequest = new AzureAccessTokenRequest(tenantId: ""
                  , clientId: ""
                  , clientSecret: "");
            var token = await context.CallActivityAsync<AzureAuthentication>($"{nameof(SharedActivity)}_{nameof(SharedActivity.GetAccessToken)}", tokenRequest);

            // TODO 取得したアクセストークンを使用して、CostManagement API を呼び出す。(3つ)
            var collectTasks = new[]
                               {
                                   context.CallActivityAsync<TotalCostResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.DailyTotalCost)}", token)
                                 , context.CallActivityAsync<TotalCostResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.WeeklyTotalCost)}", token)
                                 , context.CallActivityAsync<TotalCostResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.MonthlyTotalCost)}", token)
                               };
            var totalCostResults = await Task.WhenAll(collectTasks);

            // TODO 送信用のメッセージ形式にフォーマットする。
            chatworkMessage = await context.CallActivityAsync<ChatworkMessage>($"{nameof(SharedActivity)}_{nameof(SharedActivity.FormatChatworkMessage)}", totalCostResults);
        }
        catch (Exception e)
        {
            // 失敗した場合でもチャットに失敗したことの通知は出したい。
            // でなければ成功したのか、実行されていないのか判断できないので。
            chatworkMessage = new ChatworkMessage("Azure 利用料金の通知に失敗しました。");
            log.LogError(e, $"Failed aggregate azure cost.[{chatworkMessage}]");
        }
        finally
        {
            // TODO チャットに結果をまとめて送信する。
            chatworkSendResult = await context.CallActivityAsync<ChatworkSendResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.SendChatwork)}", chatworkMessage);
        }

        return chatworkSendResult.Log;
    }

    [FunctionName($"{nameof(Aggregate)}_{nameof(HttpStart)}")]
    public async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "api/cost/aggregate")]
            HttpRequestMessage req
          , [DurableClient] IDurableOrchestrationClient starter
          , ILogger                                     log)
    {
        var instanceId = await starter.StartNewAsync($"{nameof(Aggregate)}_{nameof(Orchestrator)}", null);

        log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

        return starter.CreateCheckStatusResponse(req, instanceId);
    }
}