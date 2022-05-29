namespace Azure.Cost.Notification.Infrastructure.ChatworkApi;

using global::ChatworkApi;

public static class Factories
{
    public static IUnitOfWork UnitOfWork(HttpClient httpClient) => new UnitOfWork(new Client(httpClient));
}