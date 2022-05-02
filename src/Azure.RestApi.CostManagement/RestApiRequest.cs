namespace Azure.RestApi.CostManagement;

using System.Text;
using System.Text.Json;
using Cysharp.Web;

internal sealed class RestApiRequest<T>
{
    public RestApiRequest(string uri, HttpContent content)
    {
        Uri     = uri;
        Content = content;
    }

    public HttpContent Content { get; }

    public string Uri { get; }

    public static RestApiRequest<T> AsFormUrlEncodedContent(string uri, T content) => new(uri, WebSerializer.ToHttpContent(content));

    public static RestApiRequest<T> AsStringContent(string uri, T content)
    {
        var json = JsonSerializer.Serialize(content, Constants.JsonSerializerOptions);
        return new(uri, new StringContent(json, Encoding.UTF8, "application/json"));
    }
}