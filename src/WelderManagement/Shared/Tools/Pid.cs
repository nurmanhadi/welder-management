namespace WelderManagement.Shared.Tools;

public static class PId
{
    public static string Generate()
    {
        var today = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        return $"PRJ-{today}";
    }
}
