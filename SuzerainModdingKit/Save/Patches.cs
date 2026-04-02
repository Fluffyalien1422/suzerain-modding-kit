using System.Text.Json;
using HarmonyLib;
using Il2Cpp;
using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;

namespace SuzerainModdingKit.Save;

[HarmonyPatch(typeof(JsonSaveLoad), nameof(JsonSaveLoad.SaveDataToFile))]
internal static class JsonSaveLoad_SaveDataToFile_Patch
{
    public static void Postfix(string path)
    {
        Dictionary<string, object> variables = [];
        try
        {
            foreach (string variableName in Variables.RegisteredVariables)
            {
                if (!DialogueLua.DoesVariableExist(variableName))
                {
                    // silently skip if it doesn't exist.
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

        //TODO: rework this.
        if (variables.Count == 0)
        {
            // We have to make sure we don't create an empty save file
            // because Suzerain sometimes calls this function before loading the save,
            // and we don't want to overwrite the existing save (if there is one).
            return;
        }

        ModSaveData modSaveData = new()
        {
            Variables = variables,
        };
        string json = JsonSerializer.Serialize(modSaveData);

        string fileName = Path.GetFileName(path);
        string savePath = Path.Combine(ModdingKitConstants.ModSavePath, fileName);

        Directory.CreateDirectory(ModdingKitConstants.ModSavePath);
        File.WriteAllText(savePath, json);

        Melon<Core>.Logger.Msg($"Saved mod data to '{savePath}'.");
    }
}

[HarmonyPatch(typeof(PersistenceManager), nameof(PersistenceManager.LoadSaveFile))]
internal static class PersistenceManager_LoadSaveFile_Patch
{
    public static void Postfix(SaveFile saveFile)
    {
        string fileName = Path.GetFileName(saveFile.path);
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
                JsonElement value = (JsonElement)entry.Value;
                switch (value.ValueKind)
                {
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        Variables.Set(entry.Key, value.GetBoolean());
                        break;
                    case JsonValueKind.String:
                        Variables.Set(entry.Key, value.GetString());
                        break;
                    case JsonValueKind.Number:
                        Variables.Set(entry.Key, value.GetSingle());
                        break;
                    case JsonValueKind.Undefined:
                    case JsonValueKind.Object:
                    case JsonValueKind.Array:
                    case JsonValueKind.Null:
                    default:
                        Melon<Core>.Logger.Warning($"Game variable '{entry.Key}' not loaded. Invalid type.");
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Melon<Core>.Logger.Warning($"Failed to load game variables from mod save at '{savePath}': {e}");
            return;
        }

        Melon<Core>.Logger.Msg($"Loaded mod data from '{savePath}'.");
    }
}
