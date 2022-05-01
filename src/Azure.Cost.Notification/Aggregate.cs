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
            // TODO �A�N�Z�X�g�[�N�����擾����B
            var tokenRequest = new AzureAccessTokenRequest(tenantId: ""
                  , clientId: ""
                  , clientSecret: "");
            var token = await context.CallActivityAsync<AzureAuthentication>($"{nameof(SharedActivity)}_{nameof(SharedActivity.GetAccessToken)}", tokenRequest);

            // TODO �擾�����A�N�Z�X�g�[�N�����g�p���āACostManagement API ���Ăяo���B(3��)
            var collectTasks = new[]
                               {
                                   context.CallActivityAsync<TotalCostResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.DailyTotalCost)}", token)
                                 , context.CallActivityAsync<TotalCostResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.WeeklyTotalCost)}", token)
                                 , context.CallActivityAsync<TotalCostResult>($"{nameof(SharedActivity)}_{nameof(SharedActivity.MonthlyTotalCost)}", token)
                               };
            var totalCostResults = await Task.WhenAll(collectTasks);

            // TODO ���M�p�̃��b�Z�[�W�`���Ƀt�H�[�}�b�g����B
            chatworkMessage = await context.CallActivityAsync<ChatworkMessage>($"{nameof(SharedActivity)}_{nameof(SharedActivity.FormatChatworkMessage)}", totalCostResults);
        }
        catch (Exception e)
        {
            // ���s�����ꍇ�ł��`���b�g�Ɏ��s�������Ƃ̒ʒm�͏o�������B
            // �łȂ���ΐ��������̂��A���s����Ă��Ȃ��̂����f�ł��Ȃ��̂ŁB
            chatworkMessage = new ChatworkMessage("Azure ���p�����̒ʒm�Ɏ��s���܂����B");
            log.LogError(e, $"Failed aggregate azure cost.[{chatworkMessage}]");
        }
        finally
        {
            // TODO �`���b�g�Ɍ��ʂ��܂Ƃ߂đ��M����B
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