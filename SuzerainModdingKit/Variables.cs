using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;

namespace SuzerainModdingKit;

/// <summary>
/// Static interface for reading and writing game variables.
/// </summary>
public static class Variables
{
    internal static List<string> RegisteredVariables = [];

    /// <summary>
    /// Registers a game variable so Suzerain Modding Kit can save/load it.
    /// </summary>
    /// <param name="name">
    /// The name of the variable.
    /// Recommended style: PascalCaseModName.PascalCaseVariableName (eg. MyMod.MyVariable).
    /// </param>
    /// <returns>
    /// A boolean indicating whether the operation succeeded.
    /// </returns>
    public static bool Register(string name)
    {
        if (RegisteredVariables.Contains(name, StringComparer.Ordinal))
        {
            Melon<Core>.Logger.Warning($"The game variable '{name}' has already been registered.");
            return false;
        }

        RegisteredVariables.Add(name);
        return true;
    }

    /// <summary>
    /// Get a bool variable. This can be a Suzerain variable or a custom variable.
    /// </summary>
    /// <param name="name">
    /// The name of the variable.
    /// </param>
    /// <returns>
    /// The value of the variable casted to a bool.
    /// </returns>
    public static bool GetBool(string name)
    {
        return DialogueLua.GetVariable(name).AsBool;
    }

    /// <summary>
    /// Get an int variable. This can be a Suzerain variable or a custom variable.
    /// </summary>
    /// <param name="name">
    /// The name of the variable.
    /// </param>
    /// <returns>
    /// The value of the variable casted to an int.
    /// </returns>
    public static int GetInt(string name)
    {
        return DialogueLua.GetVariable(name).AsInt;
    }

    /// <summary>
    /// Get a float variable. This can be a Suzerain variable or a custom variable.
    /// </summary>
    /// <param name="name">
    /// The name of the variable.
    /// </param>
    /// <returns>
    /// The value of the variable casted to a float.
    /// </returns>
    public static float GetFloat(string name)
    {
        return DialogueLua.GetVariable(name).AsFloat;
    }

    /// <summary>
    /// Get a string variable. This can be a Suzerain variable or a custom variable.
    /// </summary>
    /// <param name="name">
    /// The name of the variable.
    /// </param>
    /// <returns>
    /// The value of the variable casted to a string.
    /// </returns>
    public static string GetString(string name)
    {
        return DialogueLua.GetVariable(name).AsString;
    }

    /// <summary>
    /// Set a variable. This can be a Suzerain variable or a custom variable.
    /// </summary>
    /// <param name="name">
    /// The name of the variable.
    /// </param>
    /// <param name="value">
    /// The new value of the variable. Supported types: bool, int, float, string.
    /// </param>
    /// <returns>
    /// A boolean indicating whether the operation succeeded.
    /// </returns>
    public static bool Set(string name, object value)
    {
        if (value is bool b)
        {
            DialogueLua.SetVariable(name, b);
            return true;
        }
        if (value is int i)
        {
            DialogueLua.SetVariable(name, i);
            return true;
        }
        if (value is float f)
        {
            DialogueLua.SetVariable(name, f);
            return true;
        }
        if (value is string s)
        {
            DialogueLua.SetVariable(name, s);
            return true;
        }

        Melon<Core>.Logger.Warning($"Game variable '{name}' not set. Invalid type.");
        return false;
    }
}
