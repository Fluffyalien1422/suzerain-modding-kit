using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;

namespace SuzerainModdingKit;

public static class Variables
{
    internal static List<string> RegisteredVariables = [];

    public static void Register(string name)
    {
        if (RegisteredVariables.Contains(name, StringComparer.Ordinal))
        {
            Melon<Core>.Logger.Warning($"The game variable '{name}' has already been registered.");
            return;
        }

        RegisteredVariables.Add(name);
    }

    public static bool GetBool(string name)
    {
        return DialogueLua.GetVariable(name).AsBool;
    }

    public static int GetInt(string name)
    {
        return DialogueLua.GetVariable(name).AsInt;
    }

    public static float GetFloat(string name)
    {
        return DialogueLua.GetVariable(name).AsFloat;
    }

    public static string GetString(string name)
    {
        return DialogueLua.GetVariable(name).AsString;
    }

    public static void Set(string name, object value)
    {
        if (value is bool b)
        {
            DialogueLua.SetVariable(name, b);
        }
        else if (value is int i)
        {
            DialogueLua.SetVariable(name, i);
        }
        else if (value is float f)
        {
            DialogueLua.SetVariable(name, f);
        }
        else if (value is string s)
        {
            DialogueLua.SetVariable(name, s);
        }
        else
        {
            Melon<Core>.Logger.Warning($"Game variable '{name}' not set. Invalid type.");
        }
    }
}
