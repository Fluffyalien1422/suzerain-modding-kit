using System.Text.Json;
using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;

namespace SuzerainModdingKit.Save;

internal static class SaveManager
{
    private static bool _firstLoadSaveCalled;
    private static string _loadedSaveName;

    public static void CleanupOrphanedModSaves()
    {
        if (!Directory.Exists(ModdingKitConstants.ModSavePath))
        {
            return;
        }

        HashSet<string> suzerainSaveNames = Directory
            .GetFiles(ModdingKitConstants.SuzerainSavePath, "*.json", SearchOption.TopDirectoryOnly)
            .Select(Path.GetFileName)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        string[] modSavePaths = Directory.GetFiles(ModdingKitConstants.ModSavePath, "*.json", SearchOption.TopDirectoryOnly);

        foreach (string filePath in modSavePaths)
        {
            string fileName = Path.GetFileName(filePath);
            if (!suzerainSaveNames.Contains(fileName))
            {
                File.Delete(filePath);
            }
        }
    }

    public static void OnSuzerainLoadSaveFile(string fileName, bool isActiveSave)
    {
        if (_firstLoadSaveCalled && isActiveSave)
        {
            // Suzerain first loads the selected save, then loads the active save.
            // Reload the initial save if we have already loaded a save and the active
            // save is being loaded.
            _firstLoadSaveCalled = false;
            if (_loadedSaveName == null)
            {
                Melon<Core>.Logger.Warning("Loaded save name is null. Cannot load mod save.");
                return;
            }
            LoadSave(_loadedSaveName);
            return;
        }

        _firstLoadSaveCalled = true;
        LoadSave(fileName);
    }

    public static void Save(string fileName)
    {
        string savePath = Path.Combine(ModdingKitConstants.ModSavePath, fileName);

        Dictionary<string, object> variables = [];
        try
        {
            foreach (string variableName in Variables.RegisteredVariables)
            {
                if (!DialogueLua.DoesVariableExist(variableName))
                {
                    // Silently skip if it doesn't exist.
                    continue;
                }

                Lua.Result valueLua = DialogueLua.GetVariable(variableName);
                if (valueLua.IsBool)
                {
                    variables[variableName] = valueLua.AsBool;
                }
                else if (valueLua.IsString)
                {
                    variables[variableName] = valueLua.AsString;
                }
                else if (valueLua.IsNumber)
                {
                    variables[variableName] = valueLua.AsFloat;
                }
                else
                {
                    Melon<Core>.Logger.Warning($"Game variable '{variableName}' not saved. Invalid type.");
                }
            }
        }
        catch (Exception e)
        {
            Melon<Core>.Logger.Warning($"Failed to save mod data: {e}");
            return;
        }

        if (variables.Count == 0)
        {
            // There is nothing to save. Don't create an empty save file.
            return;
        }

        ModSaveData modSaveData = new()
        {
            Variables = variables,
        };
        string json = JsonSerializer.Serialize(modSaveData);

        Directory.CreateDirectory(ModdingKitConstants.ModSavePath);
        File.WriteAllText(savePath, json);

        Melon<Core>.Logger.Msg($"Saved mod data to '{savePath}'.");
    }

    private static void LoadSave(string fileName)
    {
        _loadedSaveName = fileName;
        string savePath = Path.Combine(ModdingKitConstants.ModSavePath, fileName);

        if (!File.Exists(savePath))
        {
            return;
        }

        string json = File.ReadAllText(savePath);
        ModSaveData modSaveData;
        try
        {
            modSaveData = JsonSerializer.Deserialize<ModSaveData>(json);
        }
        catch (Exception e)
        {
            Melon<Core>.Logger.Warning($"Failed to deserialize mod save at '{savePath}': {e}");
            return;
        }

        if (modSaveData == null || modSaveData.Variables == null)
        {
            Melon<Core>.Logger.Warning($"Invalid mod save at '{savePath}'.");
            return;
        }

        try
        {
            foreach (KeyValuePair<string, object> entry in modSaveData.Variables)
            {
                SetVariableJsonElement(entry.Key, (JsonElement)entry.Value);
            }
        }
        catch (Exception e)
        {
            Melon<Core>.Logger.Warning($"Failed to load game variables from mod save at '{savePath}': {e}");
            return;
        }

        Melon<Core>.Logger.Msg($"Loaded mod data from '{savePath}'.");
    }

    private static void SetVariableJsonElement(string variableName, JsonElement value)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.True:
            case JsonValueKind.False:
                Variables.Set(variableName, value.GetBoolean());
                break;
            case JsonValueKind.String:
                Variables.Set(variableName, value.GetString());
                break;
            case JsonValueKind.Number:
                Variables.Set(variableName, value.GetSingle());
                break;
            case JsonValueKind.Undefined:
            case JsonValueKind.Object:
            case JsonValueKind.Array:
            case JsonValueKind.Null:
            default:
                Melon<Core>.Logger.Warning($"Game variable '{variableName}' not loaded. Invalid type.");
                break;
        }
    }
}
