namespace Azure.Cost.Notification.Infrastructure.RestApi;

using Domain.Repositories;

public interface IUnitOfWork
{
    ILoginRepository LoginRepository { get; }

    IResourceUsageRepository ResourceUsageRepository { get; }
}