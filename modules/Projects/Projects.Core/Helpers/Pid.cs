namespace Projects.Core.Helpers;

public static class PId
{
    public static string Generate()
    {
        var today = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds;
        return $"PRJ-{today}";
    }
}
