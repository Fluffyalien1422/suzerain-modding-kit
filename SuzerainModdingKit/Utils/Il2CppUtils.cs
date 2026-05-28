namespace SuzerainModdingKit.Utils;

internal static class Il2CppUtils
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

    public static List<T> ListFromIl2CppList<T>(Il2CppSystem.Collections.Generic.List<T> il2CppList)
    {
        List<T> list = [];
        foreach (T v in il2CppList)
        {
            list.Add(v);
        }
        return list;
    }
}
