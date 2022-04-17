namespace Azure.Cost.Notification;

using System.Threading.Tasks;
using Domain.ValueObjects;
using Infrastructure.RestApi;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Models;

public sealed class SharedActivity
{
    private readonly IUnitOfWork _unitOfWork;

    public SharedActivity(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    //[FunctionName($"{nameof(SharedActivity)}_{nameof(SayHello)}")]
    //public string SayHello([ActivityTrigger] string name, ILogger log)
    //{
    //    log.LogInformation($"Saying hello to {name}.");
    //    return $"Hello {name}!";
    //}

    [FunctionName($"{nameof(SharedActivity)}_{nameof(GetAccessToken)}")]
    public async Task<AzureAuthentication> GetAccessToken([ActivityTrigger] AzureAccessTokenRequest request, ILogger log)
    {
        var response = await _unitOfWork.LoginRepository.Authenticate(request.TenantId, request.ClientId, request.ClientSecret).ConfigureAwait(false);

        return response;
    }
}