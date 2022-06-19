namespace Azure.Cost.Notification.Exceptions;

using System;

public sealed class ParameterNotExistException : Exception
{
    public ParameterNotExistException(string name)
            : base($"パラメータ[{name}]が指定されていません。[{name}]に値を設定してください。")
    {
    }
}