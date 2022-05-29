namespace Azure.Cost.Notification;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Domain.Models;
using Application.Domain.Services;
using Domain.ValueObjects;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public sealed class SharedActivity
{
    private readonly IAccessTokenRequestService _accessTokenRequestService;

    private readonly IUsageCostRequestService _usageCostRequestService;

    private readonly ICostMessageBuildService _costMessageBuildService;

    private readonly ISendMessageService _sendMessageService;

    public SharedActivity(IAccessTokenRequestService accessTokenRequestService
                        , IUsageCostRequestService   usageCostRequestService
                        , ICostMessageBuildService   costMessageBuildService
                        , ISendMessageService        sendMessageService)
    {
        _accessTokenRequestService = accessTokenRequestService;
        _usageCostRequestService   = usageCostRequestService;
        _costMessageBuildService   = costMessageBuildService;
        _sendMessageService        = sendMessageService;
    }

    /// <summary>
    /// Azure から OAuth を使用してアクセストークンを取得するためのアクティビティ関数です。
    /// </summary>
    /// <param name="request">アクセストークンを取得するための認証情報要求オブジェクト</param>
    /// <param name="log"></param>
    /// <returns>Azure から取得したアクセストークンを含む認証情報を返します。</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(GetAccessToken)}")]
    public async Task<AzureAuthentication> GetAccessToken([ActivityTrigger] AzureAccessTokenRequest request, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(GetAccessToken)}] Azure から OAuth を使用してアクセストークンを取得する。");

        return await _accessTokenRequestService.GetAsync(request).ConfigureAwait(false);
    }

    /// <summary>
    /// Azure Cost Management から直近１日分の利用料金情報を取得するためのアクティビティ関数です。
    /// </summary>
    /// <param name="subscriptionId">利用料金を取得する対象のサブスクリプション ID</param>
    /// <param name="log"></param>
    /// <returns>Azure から取得した１日分の利用料金情報を返します。</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(DailyTotalCost)}")]
    public async Task<TotalCostResult> DailyTotalCost([ActivityTrigger] string subscriptionId, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(DailyTotalCost)}] ");

        var dailyCost = await _usageCostRequestService.GetDailyCostAsync(subscriptionId: subscriptionId).ConfigureAwait(false);

        return new TotalCostResult(dailyCost);
    }

    /// <summary>
    /// Azure Cost Managemnt から直近支払い有効期間１週間分の料金利用情報を取得するためのアクティビティ関数です。
    /// </summary>
    /// <param name="subscriptionId">利用料金を取得する対象のサブスクリプション ID</param>
    /// <param name="log"></param>
    /// <returns>Azure から取得した１週間分の利用料金情報を返します。</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(WeeklyTotalCost)}")]
    public async Task<TotalCostResult> WeeklyTotalCost([ActivityTrigger] string subscriptionId, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(WeeklyTotalCost)}] ");

        var weeklyCost = await _usageCostRequestService.GetWeeklyCostAsync(subscriptionId: subscriptionId).ConfigureAwait(false);

        return new TotalCostResult(weeklyCost);
    }

    /// <summary>
    /// Azure Cost Management から直近支払い有効期間１ヶ月分の利用料金情報を取得するためのアクティビティ関数です。
    /// </summary>
    /// <param name="subscriptionId">利用料金を取得する対象のサブスクリプション ID</param>
    /// <param name="log"></param>
    /// <returns>Azure から取得した１ヶ月分の利用料金情報を返します。</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(MonthlyTotalCost)}")]
    public async Task<TotalCostResult> MonthlyTotalCost([ActivityTrigger] string subscriptionId, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(MonthlyTotalCost)}] ");

        var monthlyCost = await _usageCostRequestService.GetMonthlyCostAsync(subscriptionId: subscriptionId).ConfigureAwait(false);

        return new TotalCostResult(monthlyCost);
    }

    /// <summary>
    /// 収集した利用料金情報から Chatwork に送信するメッセージを書式化するためのアクティビティ関数です。
    /// </summary>
    /// <param name="totalCosts">収集した利用料金情報の配列</param>
    /// <param name="log"></param>
    /// <returns>送信するためのメッセージ情報を返します。</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(FormatChatworkMessage)}")]
    public IEnumerable<ChatworkMessage> FormatChatworkMessage([ActivityTrigger] TotalCostResult[] totalCosts, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(FormatChatworkMessage)}] ");

        return _costMessageBuildService.Build(totalCosts, TODO);
    }

    /// <summary>
    /// Chatwork にメッセージを送信するためのアクティビティ関数です。
    /// </summary>
    /// <param name="message">送信メッセージ</param>
    /// <param name="log"></param>
    /// <returns>送信結果を返します。</returns>
    /// <exception cref="NotImplementedException"></exception>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(SendChatwork)}")]
    public async Task<IEnumerable<ChatworkSendResult>> SendChatwork([ActivityTrigger] IEnumerable<ChatworkMessage> message, ILogger log)
    {
        //TODO ここを実装する。
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(SendChatwork)}] ");

        return await _sendMessageService.ExecuteAsync(message);
    }

}