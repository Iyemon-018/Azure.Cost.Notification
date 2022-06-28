namespace Azure.Cost.Notification.Tests.Domain.Entities;

using ChainingAssertion;
using Notification.Domain.Entities;
using Notification.Domain.Models;
using Xunit;

public class ChatworkSendResultTest
{
    [Fact]
    public void Test_ctor_プロパティの値がコンストラクタで設定した値を使用しているかどうか()
    {
        var target = new ChatworkSendResult(new ChatworkMessage(12345, nameof(Test_ctor_プロパティの値がコンストラクタで設定した値を使用しているかどうか)), "Test-From-Entities");

        target.Message.RoomId.Is(12345);
        target.Message.Message.Is(nameof(Test_ctor_プロパティの値がコンストラクタで設定した値を使用しているかどうか));
        target.Id.Is("Test-From-Entities");
    }

    [Theory]
    [InlineData(nameof(ChatworkSendResultTest) + nameof(Test_Equals_Idの値によって比較結果が一致するかどうか))]
    [InlineData("")]
    [InlineData(" ")]
    public void Test_Equals_Idの値によって比較結果が一致するかどうか(string messageId)
    {
        var target = new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_Equals_Idの値によって比較結果が一致するかどうか)), messageId);

        target.Equals(new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_Equals_Idの値によって比較結果が一致するかどうか)), messageId)).IsTrue();
    }

    [Theory]
    [InlineData(null)]
    public void Test_Equals_Idの値によって比較結果が不一致になるかどうか(string messageId)
    {
        var target = new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_Equals_Idの値によって比較結果が不一致になるかどうか)), messageId);

        target.Equals(new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_Equals_Idの値によって比較結果が不一致になるかどうか)), messageId)).IsFalse();
    }

    [Theory]
    [InlineData(nameof(ChatworkSendResultTest) + nameof(Test_Equals_operator_Idの値によって比較結果が一致するかどうか))]
    [InlineData("")]
    [InlineData(" ")]
    public void Test_Equals_operator_Idの値によって比較結果が一致するかどうか(string messageId)
    {
        var target = new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_Equals_operator_Idの値によって比較結果が一致するかどうか)), messageId);

        (target == new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_Equals_operator_Idの値によって比較結果が一致するかどうか)), messageId)).IsTrue();
    }

    [Theory]
    [InlineData(null)]
    public void Test_Equals_operator_Idの値によって比較結果が不一致になるかどうか(string messageId)
    {
        var target = new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_Equals_operator_Idの値によって比較結果が不一致になるかどうか)), messageId);

        (target == new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_Equals_operator_Idの値によって比較結果が不一致になるかどうか)), messageId)).IsFalse();
    }

    [Theory]
    [InlineData(nameof(ChatworkSendResultTest) + nameof(Test_NotEquals_operator_Idの値によって比較結果が一致するかどうか))]
    [InlineData("")]
    [InlineData(" ")]
    public void Test_NotEquals_operator_Idの値によって比較結果が一致するかどうか(string messageId)
    {
        var target = new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_NotEquals_operator_Idの値によって比較結果が一致するかどうか)), messageId);

        (target != new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_NotEquals_operator_Idの値によって比較結果が一致するかどうか)), messageId)).IsFalse();
    }

    [Theory]
    [InlineData(null)]
    public void Test_NotEquals_operator_Idの値によって比較結果が不一致になるかどうか(string messageId)
    {
        var target = new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_NotEquals_operator_Idの値によって比較結果が不一致になるかどうか)), messageId);

        (target != new ChatworkSendResult(new ChatworkMessage(12346, nameof(Test_NotEquals_operator_Idの値によって比較結果が不一致になるかどうか)), messageId)).IsTrue();
    }
}