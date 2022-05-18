namespace Azure.Cost.Notification.Tests.Application.Domain.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using ChainingAssertion;
using Notification.Application.Domain.Models;
using Notification.Application.Domain.Services;
using Notification.Domain.ValueObjects;
using Xunit;
using Xunit.Abstractions;

public class MessageBuildExtensionsTest
{
    private readonly ITestOutputHelper _helper;

    public MessageBuildExtensionsTest(ITestOutputHelper helper)
    {
        _helper = helper;
    }

    public static IEnumerable<object[]> Build_Test_AsTitle_Data()
    {
        var today = DateTime.Today;
        yield return new object[]
                     {
                         new TotalCostResult(new DailyCost(today, new []{new ResourceUsage(786.34m, "Daily-Cost-Resource", "DailyTest", "0000-1111-2222-3333")}))
                         , $"{today:yyyy/MM/dd} の利用料金(Daily)"
                     };

        yield return new object[]
                     {
                         new TotalCostResult(new WeeklyCost(today.AddDays(-6), today, new []{new ResourceUsage(786.34m, "Weekly-Cost-Resource", "WeeklyTest", "0000-1111-2222-3333")}))
                       , $"{today.AddDays(-6):yyyy/MM/dd} - {today:yyyy/MM/dd} の利用料金(Weekly)"
                     };

        yield return new object[]
                     {
                         new TotalCostResult(new MonthlyCost(new []{new ResourceUsage(786.34m, "Monthly-Cost-Resource", "MonthlyTest", "0000-1111-2222-3333")}))
                       , $"今月の利用料金(Monthly)"
                     };
    }

    [Theory]
    [MemberData(nameof(Build_Test_AsTitle_Data))]
    public void Test_AsTitle_期間種別ごとのタイトル文字列がそれぞれの期間種別を表しているかどうか(TotalCostResult totalCostResult, string expected)
    {
        totalCostResult.AsTitle().Is(expected);
    }

    public static IEnumerable<object[]> Get_Test_AsResourcesCost_Data()
    {
        // リソース使用料データが５件あるケース
        yield return new object[]
                     {
                         new TotalCostResult(new DailyCost(DateTime.Today
                               , new[]
                                 {
                                     new ResourceUsage(12345.02m, "High Group", "Highest Resource", "9999")
                                   , new ResourceUsage(203.19m, "Middle Group", "Middle Resource", "5000")
                                   , new ResourceUsage(0.054m, "Low Group", "Lowest Resource", "0000")
                                   , new ResourceUsage(73.115m, "Low Group", "Lower Resource", "2500")
                                   , new ResourceUsage(9300.092230001m, "High Group", "Higher Resource", "7500")
                                 }))
                         , $"- High Group / 9999(Highest Resource) ¥12,345.02{Environment.NewLine}"
                         + $"- High Group / 7500(Higher Resource) ¥9,300.09{Environment.NewLine}"
                         + $"- Middle Group / 5000(Middle Resource) ¥203.19{Environment.NewLine}"
                         + $"- Low Group / 2500(Lower Resource) ¥73.12{Environment.NewLine}"
                         + $"- Low Group / 0000(Lowest Resource) ¥0.05"
                     };

        // リソース使用料データが４件あるケース
        yield return new object[]
                     {
                         new TotalCostResult(new DailyCost(DateTime.Today
                               , new[]
                                 {
                                     new ResourceUsage(12345.02m, "High Group", "Highest Resource", "9999")
                                   , new ResourceUsage(0.054m, "Low Group", "Lowest Resource", "0000")
                                   , new ResourceUsage(73.115m, "Low Group", "Lower Resource", "2500")
                                   , new ResourceUsage(9300.092230001m, "High Group", "Higher Resource", "7500")
                                 }))
                       , $"- High Group / 9999(Highest Resource) ¥12,345.02{Environment.NewLine}"
                       + $"- High Group / 7500(Higher Resource) ¥9,300.09{Environment.NewLine}"
                       + $"- Low Group / 2500(Lower Resource) ¥73.12{Environment.NewLine}"
                       + $"- Low Group / 0000(Lowest Resource) ¥0.05"
                     };
        
        // リソース使用料データが６件あるケース
        yield return new object[]
                     {
                         new TotalCostResult(new DailyCost(DateTime.Today
                               , new[]
                                 {
                                     new ResourceUsage(12345.02m, "High Group", "Highest Resource", "9999")
                                   , new ResourceUsage(203.19m, "Middle Group", "Middle Resource", "5000")
                                   , new ResourceUsage(0.054m, "Low Group", "Lowest Resource", "0000")
                                   , new ResourceUsage(73.115m, "Low Group", "Lower Resource", "2500")
                                   , new ResourceUsage(9300.092230001m, "High Group", "Higher Resource", "7500")
                                   , new ResourceUsage(780.028m, "Middle Group", "Middle Resource", "5000")
                                 }))
                       , $"- High Group / 9999(Highest Resource) ¥12,345.02{Environment.NewLine}"
                       + $"- High Group / 7500(Higher Resource) ¥9,300.09{Environment.NewLine}"
                       + $"- Middle Group / 5000(Middle Resource) ¥780.03{Environment.NewLine}"
                       + $"- Middle Group / 5000(Middle Resource) ¥203.19{Environment.NewLine}"
                       + $"- Low Group / 2500(Lower Resource) ¥73.12"
                     };

        // リソース使用料データが０件あるケース
        yield return new object[]
                     {
                         new TotalCostResult(new DailyCost(DateTime.Today, Enumerable.Empty<ResourceUsage>()))
                       , string.Empty
                     };
    }

    [Theory]
    [MemberData(nameof(Get_Test_AsResourcesCost_Data))]
    public void Test_AsResourcesCost(TotalCostResult totalCostResult, string expected)
    {
        totalCostResult.AsResourcesCost().Is(expected);

        _helper.WriteLine(expected);
    }
}