namespace SuzerainModdingKit;

public static class ModdingKitConstants
{
    public static readonly Version ModVersion = new(0, 1, 0);
    public static readonly string UserDir = Environment.GetEnvironmentVariable("userprofile");
    public static readonly string SuzerainSavePath = Path.Combine(UserDir, "AppData/LocalLow/Torpor Games/Suzerain");
    public static readonly string ModSavePath = Path.Combine(SuzerainSavePath, "moddingkit");
}
