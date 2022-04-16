namespace Azure.RestApi.CostManagement;

public interface IClient
{
    ILogin Login { get; }

    IQuery Query { get; }
}