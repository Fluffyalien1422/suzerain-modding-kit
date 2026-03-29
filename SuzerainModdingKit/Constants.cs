namespace SuzerainModdingKit;

internal static class Constants
{
    public static readonly string UserDir = Environment.GetEnvironmentVariable("userprofile");
    public static readonly string SuzerainSavePath = Path.Combine(UserDir, "AppData/LocalLow/Torpor Games/Suzerain");
    public static readonly string ModSavePath = Path.Combine(SuzerainSavePath, "moddingkit");
}
