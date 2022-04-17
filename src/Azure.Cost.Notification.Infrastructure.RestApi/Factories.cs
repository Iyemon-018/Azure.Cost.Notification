namespace Azure.Cost.Notification.Infrastructure.RestApi;

public static class Factories
{
    public static IUnitOfWork UnitOfWork(HttpClient httpClient) => new UnitOfWork(httpClient);
}