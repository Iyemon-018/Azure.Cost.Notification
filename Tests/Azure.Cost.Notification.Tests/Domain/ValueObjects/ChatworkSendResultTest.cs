namespace Azure.Cost.Notification.Tests.Domain.ValueObjects;

using System.Collections.Generic;
using ChainingAssertion;
using Notification.Domain.ValueObjects;
using Xunit;

public class ChatworkSendResultTest
{
    public static IEnumerable<object?[]> Get_Test_Ctor()
    {
        yield return new object?[] { 0, string.Empty, null };
        yield return new object?[] { 0, string.Empty, $"{nameof(Get_Test_Ctor)}" };
        yield return new object?[] { 1, string.Empty, $"{nameof(Get_Test_Ctor)}" };
        yield return new object?[] { 0, $"{nameof(Get_Test_Ctor)}", string.Empty };
    }

    [Theory]
    [MemberData(nameof(Get_Test_Ctor))]
    public void Test_Ctor(int roomId, string message, string messageId)
    {
        // 例外が出なけりゃいい。
        new ChatworkSendResult(new ChatworkMessage(roomId, message), messageId).Is(new ChatworkSendResult(new ChatworkMessage(roomId, message), messageId));
    }

    [Theory]
    [InlineData(0, $"{nameof(Test_Log)}", "A89203029", $"Send:A89203029, Room:0, {nameof(Test_Log)}")]
    [InlineData(90221029, $"{nameof(Test_Log)}", "_02939a___", $"Send:_02939a___, Room:90221029, {nameof(Test_Log)}")]
    [InlineData(-292883, "", "A000", $"Send:A000, Room:-292883, ")]
    [InlineData(100, $"{nameof(Test_Log)}", "", $"Send:, Room:100, {nameof(Test_Log)}")]
    public void Test_Log(int roomId, string message, string messageId, string expected)
    {
        new ChatworkSendResult(new ChatworkMessage(roomId, message), messageId).Log.Is(expected);
    }

    [Fact]
    public void Test_Equals_いずれかの値が異なる場合は一致しないこと()
    {
        var target = new ChatworkSendResult(new ChatworkMessage(1098, nameof(Test_Equals_いずれかの値が異なる場合は一致しないこと)), "A9010");

        (target == new ChatworkSendResult(new ChatworkMessage(1099, nameof(Test_Equals_いずれかの値が異なる場合は一致しないこと)), "A9010")).IsFalse("roomId が不一致");
        (target == new ChatworkSendResult(new ChatworkMessage(1098, nameof(Test_Equals_いずれかの値が異なる場合は一致しないこと) + "_"), "A9010")).IsFalse("message が不一致");
        (target == new ChatworkSendResult(new ChatworkMessage(1098, nameof(Test_Equals_いずれかの値が異なる場合は一致しないこと)), "A9009")).IsFalse("messageId が不一致");
        (target == new ChatworkSendResult(new ChatworkMessage(1097, nameof(Test_Equals_いずれかの値が異なる場合は一致しないこと) + "_"), "A9011")).IsFalse("すべて不一致");
    }

    [Fact]
    public void Test_Equals_比較対象のオブジェクトがnullの場合は一致しないこと()
    {
        var target = new ChatworkSendResult(new ChatworkMessage(0, string.Empty), "A90909090");

        (target == null).IsFalse();

        target.Equals(null).IsFalse();
    }
}