using Il2CppPixelCrushers.DialogueSystem;

namespace SuzerainModdingKit.Character;

public class CharacterNameSelector : CharacterSelector
{
    public string CharacterName
    {
        get; init;
    }

    public CharacterNameSelector(string characterName)
    {
        CharacterName = characterName ?? throw new ArgumentNullException(nameof(characterName));
    }

    public override int? Resolve()
    {
        DialogueDatabase db = DialogueManager.MasterDatabase;
        return db?.GetActor(CharacterName)?.id;
    }
}
