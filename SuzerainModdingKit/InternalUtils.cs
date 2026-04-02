namespace SuzerainModdingKit;

internal static class InternalUtils
{
    public static Il2CppSystem.Collections.Generic.List<T> CreateIl2CppList<T>(List<T> list)
    {
        Il2CppSystem.Collections.Generic.List<T> il2CppList = new();
        foreach (T v in list)
        {
            il2CppList.Add(v);
        }
        return il2CppList;
    }
}
