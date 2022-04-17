namespace Azure.Cost.Notification.Domain.ValueObjects;

public sealed class AzureAuthentication : ValueObjectBase<AzureAuthentication>
{
    public AzureAuthentication(long expiredOn, long notBefore, string tokenType, string accessToken)
    {
        ExpiredOn   = DateTimeOffset.FromUnixTimeSeconds(expiredOn).LocalDateTime;
        NotBefore   = DateTimeOffset.FromUnixTimeSeconds(notBefore).LocalDateTime;
        TokenType   = tokenType;
        AccessToken = accessToken;
    }

    protected override bool EqualsCore(AzureAuthentication other)
        => ExpiredOn == other.ExpiredOn
        && NotBefore == other.NotBefore
        && TokenType == other.TokenType
        && AccessToken == other.AccessToken;

    /// <summary>
    /// サービスプリンシパルの有効期限終了日時を取得します。
    /// </summary>
    public DateTime ExpiredOn { get; }

    /// <summary>
    /// サービスプリンシパルの有効期限開始日時を取得します。
    /// </summary>
    public DateTime NotBefore { get; }

    public string TokenType { get; }

    public string AccessToken { get; }
}