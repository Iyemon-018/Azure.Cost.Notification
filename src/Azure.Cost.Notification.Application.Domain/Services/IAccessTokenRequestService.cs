namespace Azure.Cost.Notification.Application.Domain.Services;

using Models;
using Notification.Domain.Models;

/// <summary>
/// Azure からアクセストークンを取得するためのアプリケーション サービス インターフェースです。
/// </summary>
public interface IAccessTokenRequestService
{
    public Task<AzureAuthentication> GetAsync(AzureAccessTokenRequest request);
}