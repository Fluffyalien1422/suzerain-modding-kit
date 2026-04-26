namespace SuzerainModdingKit.Utils;

internal static class ArticyIdGenerator
{
    public static string GenerateArticyId(string seed)
    {
        ulong hash = 0x0FFFFFFF00000000UL;
        foreach (char c in seed)
        {
            hash = (hash * 31) + c;
        }

        // Keep it in the safe mod namespace range.
        hash = 0x0FFFFFFF00000000UL | (hash & 0x00000000FFFFFFFFUL);

        return $"0x{hash:X16}";
    }
}
