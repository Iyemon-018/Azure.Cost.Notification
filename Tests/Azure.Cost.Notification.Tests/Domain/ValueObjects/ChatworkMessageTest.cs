namespace Azure.Cost.Notification.Tests.Domain.ValueObjects;

using System;
using System.Collections.Generic;
using ChainingAssertion;
using Notification.Domain.ValueObjects;
using Xunit;

public class ChatworkMessageTest
{
    public static IEnumerable<object?[]> Get_Test_Ctor()
    {
        yield return new object?[] {0, string.Empty};
        yield return new object?[] {0, null};
        yield return new object?[] {0, " "};
        yield return new object?[] {0, Constants.MaximumLengthString};
        yield return new object?[] {int.MinValue, string.Empty};
        yield return new object?[] {int.MaxValue, string.Empty};
    }

    [Theory]
    [MemberData(nameof(Get_Test_Ctor))]
    public void Test_Ctor(int roomId, string message)
    {
        // 例外が出なけりゃいい。
        new ChatworkMessage(roomId, message).Is(new ChatworkMessage(roomId, message));
    }

    public static IEnumerable<object[]> Get_Test_ToString()
    {
        yield return new object[] { $"{nameof(Test_ToString)}", $"{nameof(Test_ToString)}" };

        var now = $"{DateTime.Now:yyyy/MM/DD_HH:mm:ss.fff}";
        yield return new object[] { now, now };
    }

    [Theory]
    [MemberData(nameof(Get_Test_ToString))]
    public void Test_ToString(string message, string expected)
    {
        new ChatworkMessage(0, message).ToString().Is(expected);
    }

    [Fact]
    public void Test_Equals_いずれかの値が異なる場合は一致しないこと()
    {
        var target = new ChatworkMessage(1098, nameof(Test_Equals_いずれかの値が異なる場合は一致しないこと));

        (target == new ChatworkMessage(1099, nameof(Test_Equals_いずれかの値が異なる場合は一致しないこと))).IsFalse("roomId が不一致");
        (target == new ChatworkMessage(1098, nameof(Test_Equals_いずれかの値が異なる場合は一致しないこと) + "_")).IsFalse("message が不一致");
        (target == new ChatworkMessage(1097, nameof(Test_Equals_いずれかの値が異なる場合は一致しないこと) + "_")).IsFalse("両方不一致");
    }

    [Fact]
    public void Test_Equals_比較対象のオブジェクトがnullの場合は一致しないこと()
    {
        var target = new ChatworkMessage(0, string.Empty);

        (target == null).IsFalse();

        target.Equals(null).IsFalse();
    }
}