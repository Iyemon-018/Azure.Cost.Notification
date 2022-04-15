namespace Azure.RestApi.CostManagement;

public sealed partial class Client : IClient
{
    public IQuery Query => this;
}