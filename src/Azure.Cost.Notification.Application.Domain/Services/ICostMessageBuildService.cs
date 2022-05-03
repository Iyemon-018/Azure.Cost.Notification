namespace Azure.Cost.Notification.Application.Domain.Services;

using Models;

/// <summary>
/// 収集した利用料金の情報を送信用のメッセージ形式に変換するためのアプリケーション サービス インターフェースです。
/// </summary>
public interface ICostMessageBuildService
{
    IEnumerable<ChatworkMessage> Build(TotalCostResult[] totalCosts);
}