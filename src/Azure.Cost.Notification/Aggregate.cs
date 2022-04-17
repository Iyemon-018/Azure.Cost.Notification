namespace Azure.Cost.Notification;

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Domain.ValueObjects;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Models;

public sealed class Aggregate
{
    [FunctionName($"{nameof(Aggregate)}_{nameof(Orchestrator)}")]
    public async Task<List<string>> Orchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
    {
        var outputs = new List<string>();

        // TODO アクセストークンを取得する。

        // TODO 取得したアクセストークンを使用して、CostManagement API を呼び出す。(3つ)

        // TODO 送信用のメッセージ形式にフォーマットする。

        // TODO チャットに結果をまとめて送信する。

        // Replace "hello" with the name of your Durable Activity Function.
        //outputs.Add(await context.CallActivityAsync<string>($"{nameof(SharedActivity)}_{nameof(SharedActivity.SayHello)}", "Tokyo"));
        //outputs.Add(await context.CallActivityAsync<string>($"{nameof(SharedActivity)}_{nameof(SharedActivity.SayHello)}", "Seattle"));
        //outputs.Add(await context.CallActivityAsync<string>($"{nameof(SharedActivity)}_{nameof(SharedActivity.SayHello)}", "London"));

        var tokenRequest = new AzureAccessTokenRequest(tenantId: ""
              , clientId: ""
              , clientSecret: "");
        var token = await context.CallActivityAsync<AzureAuthentication>($"{nameof(SharedActivity)}_{nameof(SharedActivity.GetAccessToken)}", tokenRequest);
        // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
        return outputs;
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