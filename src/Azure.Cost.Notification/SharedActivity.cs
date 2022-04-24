namespace Azure.Cost.Notification;

using System;
using System.Threading.Tasks;
using Domain.ValueObjects;
using Infrastructure.RestApi;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Models;

public sealed class SharedActivity
{
    private readonly IUnitOfWork _unitOfWork;

    public SharedActivity(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
        return await _unitOfWork.LoginRepository.Authenticate(request.TenantId, request.ClientId, request.ClientSecret).ConfigureAwait(false);
    }

    /// <summary>
    /// Azure Cost Management から直近１日分の利用料金情報を取得するためのアクティビティ関数です。
    /// </summary>
    /// <param name="authentication">Azure REST API を使用するための認証情報</param>
    /// <param name="log"></param>
    /// <returns>Azure から取得した１日分の利用料金情報を返します。</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(DailyTotalCost)}")]
    public async Task<TotalCostResult> DailyTotalCost([ActivityTrigger] AzureAuthentication authentication, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(DailyTotalCost)}] ");
        throw new NotImplementedException();
    }

    /// <summary>
    /// Azure Cost Managemnt から直近支払い有効期間１週間分の料金利用情報を取得するためのアクティビティ関数です。
    /// </summary>
    /// <param name="authentication">Azure REST API を使用するための認証情報</param>
    /// <param name="log"></param>
    /// <returns>Azure から取得した１週間分の利用料金情報を返します。</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(WeeklyTotalCost)}")]
    public async Task<TotalCostResult> WeeklyTotalCost([ActivityTrigger] AzureAuthentication authentication, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(WeeklyTotalCost)}] ");
        throw new NotImplementedException();
    }

    /// <summary>
    /// Azure Cost Management から直近支払い有効期間１ヶ月分の利用料金情報を取得するためのアクティビティ関数です。
    /// </summary>
    /// <param name="authentication">Azure REST API を使用するための認証情報</param>
    /// <param name="log"></param>
    /// <returns>Azure から取得した１ヶ月分の利用料金情報を返します。</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(MonthlyTotalCost)}")]
    public async Task<TotalCostResult> MonthlyTotalCost([ActivityTrigger] AzureAuthentication authentication, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(MonthlyTotalCost)}] ");
        throw new NotImplementedException();
    }

    /// <summary>
    /// 収集した利用料金情報から Chatwork に送信するメッセージを書式化するためのアクティビティ関数です。
    /// </summary>
    /// <param name="totalCosts">収集した利用料金情報の配列</param>
    /// <param name="log"></param>
    /// <returns>送信するためのメッセージ情報を返します。</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(FormatChatworkMessage)}")]
    public async Task<ChatworkMessage> FormatChatworkMessage([ActivityTrigger] TotalCostResult[] totalCosts, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(FormatChatworkMessage)}] ");
        throw new NotImplementedException();
    }

    /// <summary>
    /// Chatwork にメッセージを送信するためのアクティビティ関数です。
    /// </summary>
    /// <param name="message">送信メッセージ</param>
    /// <param name="log"></param>
    /// <returns>送信結果を返します。</returns>
    /// <exception cref="NotImplementedException"></exception>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(SendChatwork)}")]
    public async Task<ChatworkSendResult> SendChatwork([ActivityTrigger] ChatworkMessage message, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(SendChatwork)}] ");
        throw new NotImplementedException();
    }

}

// ここから下は .Application.Domain に追加するモデル。
public sealed class ChatworkSendResult
{
    public ChatworkSendResult(ChatworkMessage message, string messageId)
    {
        Log = $"Send [{messageId}] {message}";
    }

    public string Log { get; }
}

public sealed class ChatworkMessage
{
    private readonly string _message;

    public ChatworkMessage(string message)
    {
        _message = message;
    }

    public override string ToString() => $"{_message}";
}

public sealed class TotalCostResult
{
    // 収集期間種別 みたいなので、日刊、週刊、月刊とか識別できるようにしたい。

    // ここには REST API で取得したデータがすべて入っている想定。
}