namespace Azure.Cost.Notification.Domain.Extensions;

public static class StringConvertExtensions
{
    public static int ToInt(this string self, int fallbackValue = default) => int.TryParse(self, out var result) ? result : fallbackValue;
}