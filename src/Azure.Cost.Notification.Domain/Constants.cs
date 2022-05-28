namespace Azure.Cost.Notification.Domain;

using System.Globalization;

public static class Constants
{
    private static CultureInfo _currentCulture;
    
    public static CultureInfo CultureJapanese => _currentCulture ??= CultureInfo.GetCultureInfo("ja-JP");
}