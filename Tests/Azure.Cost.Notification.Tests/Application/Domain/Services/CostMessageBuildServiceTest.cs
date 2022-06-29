namespace Azure.Cost.Notification.Tests.Application.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using ChainingAssertion;
using Notification.Application.Domain.Models;
using Notification.Application.Domain.Services;
using Notification.Domain.Entities;
using Notification.Domain.ValueObjects;
using Xunit;
using Xunit.Abstractions;

public class CostMessageBuildServiceTest
{
    private readonly ITestOutputHelper _helper;

    private readonly CostMessageBuildService _target;
    
    public CostMessageBuildServiceTest(ITestOutputHelper helper)
    {
        _helper = helper;
        _target = new CostMessageBuildService();
    }

    public static IEnumerable<object[]> Get_Test_Build_Data()
    {
        var today = DateTime.Today;
        // １種だけのケース
        yield return new object[]
                     {
                         new[]
                         {
                             new TotalCostResult(new DailyCost(today, new[] {new ResourceUsage(100.0m, "Daily Group", "Daily Cost", "01010")}))
                         }
                       , new[]
                         {
                             $"[info][title]{today:yyyy/MM/dd} の利用料金(Daily)[/title]"
                           + $"合計: ￥100.00{Environment.NewLine}"
                           + $"[hr]"
                           + $"利用料の高いリソース{Environment.NewLine}"
                           + $"1. Daily Group / 01010(Daily Cost) ￥100.00"
                           + $"[/info]※Azure.Cost.Notification による自動通知です。"
                         }
                     };

        // 全種類のケース
        yield return new object[]
                     {
                         new[]
                         {
                             new TotalCostResult(new DailyCost(today, new[] {new ResourceUsage(100.0m, "Daily Group", "Daily Cost", "01010")}))
                           , new TotalCostResult(new WeeklyCost(today.AddDays(-6), today, new[] {new ResourceUsage(101.0m, "Weekly Group", "Weekly Cost", "21010")}))
                           , new TotalCostResult(new MonthlyCost(new[] {new ResourceUsage(191.0m, "Monthly Group", "Monthly Cost", "21711")}))
                         }
                       , new[]
                         {
                             $"[info][title]{today:yyyy/MM/dd} の利用料金(Daily)[/title]"
                           + $"合計: ￥100.00{Environment.NewLine}"
                           + $"[hr]"
                           + $"利用料の高いリソース{Environment.NewLine}"
                           + $"1. Daily Group / 01010(Daily Cost) ￥100.00"
                           + $"[/info]※Azure.Cost.Notification による自動通知です。"
                           , $"[info][title]{today.AddDays(-6):yyyy/MM/dd} - {today:yyyy/MM/dd} の利用料金(Weekly)[/title]"
                           + $"合計: ￥101.00{Environment.NewLine}"
                           + $"[hr]"
                           + $"利用料の高いリソース{Environment.NewLine}"
                           + $"1. Weekly Group / 21010(Weekly Cost) ￥101.00"
                           + $"[/info]※Azure.Cost.Notification による自動通知です。"
                           , $"[info][title]今月の利用料金(Monthly)[/title]"
                           + $"合計: ￥191.00{Environment.NewLine}"
                           + $"[hr]"
                           + $"利用料の高いリソース{Environment.NewLine}"
                           + $"1. Monthly Group / 21711(Monthly Cost) ￥191.00"
                           + $"[/info]※Azure.Cost.Notification による自動通知です。"
                         }
                     };
    }

    [Theory]
    [MemberData(nameof(Get_Test_Build_Data))]
    public void Test_Build_通知メッセージのフォーマットがChatwork送信用に変換できるかどうか(TotalCostResult[] totalCosts, IEnumerable<string> expected)
    {
        _target.Build(87920, totalCosts).Select(x => $"{x}").ToArray().Is(expected);
        _helper.WriteLine(string.Join(Environment.NewLine, expected));
    }
}