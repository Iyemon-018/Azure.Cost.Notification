namespace Azure.Cost.Notification.Tests.Application.Domain.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using ChainingAssertion;
using Notification.Application.Domain.Models;
using Notification.Domain.ValueObjects;
using Xunit;

public class TotalCostResultTest
{
    public static IEnumerable<ResourceUsage> Build()
        => new[]
           {
               new ResourceUsage(34.023M, "Notification.Test", "BLOB Storage", "aesio00123")
             , new ResourceUsage(150.1M, "Notification.Test", "App Service", "app-example-001")
             , new ResourceUsage(78.04M, "Application.Test", "Virtual Machine", "vm-win2019")
             , new ResourceUsage(10.202M, "Application.Test", "Virtual Network", "win2019-ip")
             , new ResourceUsage(106.6789M, "Domain.Cost", "SQL Database", "example-db-sql")
           };

    public static IEnumerable<ResourceUsage> TakeTop(int count) => Build().OrderByDescending(x => x.Cost).Take(count);

    [Fact]
    public void Test_TotalCost_Empty()
    {
        new TotalCostResult(new DailyCost(DateTime.UtcNow.Date, Enumerable.Empty<ResourceUsage>())).TotalCost().Is(0M);
    }

    [Fact]
    public void Test_DailyCost_TotalCost()
    {
        new TotalCostResult(new DailyCost(DateTime.UtcNow.Date, Build())).TotalCost().Is(379.0439M);
    }

    [Fact]
    public void Test_WeeklyCost_TotalCost()
    {
        new TotalCostResult(new WeeklyCost(DateTime.UtcNow.Date.AddDays(-6), DateTime.UtcNow.Date, Build())).TotalCost().Is(379.0439M);
    }

    [Fact]
    public void Test_MonthlyCost_TotalCost()
    {
        new TotalCostResult(new MonthlyCost(Build())).TotalCost().Is(379.0439M);
    }

    [Fact]
    public void Test_DailyCost_TakeHighAmount()
    {
        new TotalCostResult(new DailyCost(DateTime.UtcNow.Date, Build())).TakeHighAmount(3).Is(TakeTop(3));
    }

    [Fact]
    public void Test_WeeklyCost_TakeHighAmount()
    {
        new TotalCostResult(new WeeklyCost(DateTime.UtcNow.Date.AddDays(-6), DateTime.UtcNow.Date, Build())).TakeHighAmount(3).Is(TakeTop(3));
    }

    [Fact]
    public void Test_MonthlyCost_TakeHighAmount()
    {
        new TotalCostResult(new MonthlyCost(Build())).TakeHighAmount(3).Is(TakeTop(3));
    }

    [Fact]
    public void Test_TakeHighAmount_éÊÇËèoÇ∑êîÇ™ïâÇÃêîÇÃèÍçáÇ…ñﬂÇËílÇ™EmptyÇ≈Ç†ÇÈÇ±Ç∆()
    {
        new TotalCostResult(new DailyCost(DateTime.UtcNow.Date, Build())).TakeHighAmount(-1).IsEmpty();
    }
}