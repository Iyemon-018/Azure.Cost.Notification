namespace Azure.Cost.Notification;

using System;
using System.Threading.Tasks;
using Application.Domain.Models;
using Application.Domain.Services;
using Domain.ValueObjects;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

public sealed class SharedActivity
{
    private readonly IAccessTokenRequestService _accessTokenRequestService;

    public SharedActivity(IAccessTokenRequestService accessTokenRequestService)
    {
        _accessTokenRequestService = accessTokenRequestService;
    }

    /// <summary>
    /// Azure ���� OAuth ���g�p���ăA�N�Z�X�g�[�N�����擾���邽�߂̃A�N�e�B�r�e�B�֐��ł��B
    /// </summary>
    /// <param name="request">�A�N�Z�X�g�[�N�����擾���邽�߂̔F�؏��v���I�u�W�F�N�g</param>
    /// <param name="log"></param>
    /// <returns>Azure ����擾�����A�N�Z�X�g�[�N�����܂ޔF�؏���Ԃ��܂��B</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(GetAccessToken)}")]
    public async Task<AzureAuthentication> GetAccessToken([ActivityTrigger] AzureAccessTokenRequest request, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(GetAccessToken)}] Azure ���� OAuth ���g�p���ăA�N�Z�X�g�[�N�����擾����B");

        return await _accessTokenRequestService.GetAsync(request).ConfigureAwait(false);
    }

    /// <summary>
    /// Azure Cost Management ���璼�߂P�����̗��p���������擾���邽�߂̃A�N�e�B�r�e�B�֐��ł��B
    /// </summary>
    /// <param name="authentication">Azure REST API ���g�p���邽�߂̔F�؏��</param>
    /// <param name="log"></param>
    /// <returns>Azure ����擾�����P�����̗��p��������Ԃ��܂��B</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(DailyTotalCost)}")]
    public async Task<TotalCostResult> DailyTotalCost([ActivityTrigger] AzureAuthentication authentication, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(DailyTotalCost)}] ");
        throw new NotImplementedException();
    }

    /// <summary>
    /// Azure Cost Managemnt ���璼�ߎx�����L�����ԂP�T�ԕ��̗������p�����擾���邽�߂̃A�N�e�B�r�e�B�֐��ł��B
    /// </summary>
    /// <param name="authentication">Azure REST API ���g�p���邽�߂̔F�؏��</param>
    /// <param name="log"></param>
    /// <returns>Azure ����擾�����P�T�ԕ��̗��p��������Ԃ��܂��B</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(WeeklyTotalCost)}")]
    public async Task<TotalCostResult> WeeklyTotalCost([ActivityTrigger] AzureAuthentication authentication, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(WeeklyTotalCost)}] ");
        throw new NotImplementedException();
    }

    /// <summary>
    /// Azure Cost Management ���璼�ߎx�����L�����ԂP�������̗��p���������擾���邽�߂̃A�N�e�B�r�e�B�֐��ł��B
    /// </summary>
    /// <param name="authentication">Azure REST API ���g�p���邽�߂̔F�؏��</param>
    /// <param name="log"></param>
    /// <returns>Azure ����擾�����P�������̗��p��������Ԃ��܂��B</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(MonthlyTotalCost)}")]
    public async Task<TotalCostResult> MonthlyTotalCost([ActivityTrigger] AzureAuthentication authentication, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(MonthlyTotalCost)}] ");
        throw new NotImplementedException();
    }

    /// <summary>
    /// ���W�������p������񂩂� Chatwork �ɑ��M���郁�b�Z�[�W�����������邽�߂̃A�N�e�B�r�e�B�֐��ł��B
    /// </summary>
    /// <param name="totalCosts">���W�������p�������̔z��</param>
    /// <param name="log"></param>
    /// <returns>���M���邽�߂̃��b�Z�[�W����Ԃ��܂��B</returns>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(FormatChatworkMessage)}")]
    public async Task<ChatworkMessage> FormatChatworkMessage([ActivityTrigger] TotalCostResult[] totalCosts, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(FormatChatworkMessage)}] ");
        throw new NotImplementedException();
    }

    /// <summary>
    /// Chatwork �Ƀ��b�Z�[�W�𑗐M���邽�߂̃A�N�e�B�r�e�B�֐��ł��B
    /// </summary>
    /// <param name="message">���M���b�Z�[�W</param>
    /// <param name="log"></param>
    /// <returns>���M���ʂ�Ԃ��܂��B</returns>
    /// <exception cref="NotImplementedException"></exception>
    [FunctionName($"{nameof(SharedActivity)}_{nameof(SendChatwork)}")]
    public async Task<ChatworkSendResult> SendChatwork([ActivityTrigger] ChatworkMessage message, ILogger log)
    {
        log.LogInformation($"[{nameof(SharedActivity)}_{nameof(SendChatwork)}] ");
        throw new NotImplementedException();
    }

}