﻿namespace Azure.RestApi.CostManagement.Data;

using System.Net.Http.Headers;

public sealed class AccessToken
{
    public string token_type { get; set; } = null!;

    public int expires_in { get; set; }

    public int ext_expires_in { get; set; }

    public long expires_on { get; set; }

    public long not_before { get; set; }

    public string resource { get; set; } = null!;

    public string access_token { get; set; } = null!;

    internal AuthenticationHeaderValue AsHeaderValue() => new(token_type, access_token);
}