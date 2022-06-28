namespace Azure.Cost.Notification.Domain.Models;

public sealed class AzureAuthentication
{
    public AzureAuthentication()
    {
        
    }

    public AzureAuthentication(long expiredOn, long notBefore, string tokenType, string accessToken)
    {
        ExpiredOn   = expiredOn;
        NotBefore   = notBefore;
        TokenType   = tokenType;
        AccessToken = accessToken;
    }

    /// <summary>
    /// サービスプリンシパルの有効期限終了日時を取得します。
    /// </summary>
    public long ExpiredOn { get; set; }

    /// <summary>
    /// サービスプリンシパルの有効期限開始日時を取得します。
    /// </summary>
    public long NotBefore { get; set; }

    public string TokenType { get; set; }

    public string AccessToken { get; set; }

    public DateTime ExpiredOnUnixTimeSecond() => DateTimeOffset.FromUnixTimeSeconds(ExpiredOn).LocalDateTime;

    public DateTime NotBeforeUnixTimeSecond() => DateTimeOffset.FromUnixTimeSeconds(NotBefore).LocalDateTime;
}