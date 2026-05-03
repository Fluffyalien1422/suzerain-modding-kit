using Il2CppPixelCrushers.DialogueSystem;

namespace SuzerainModdingKit.Character;

/// <summary>
/// Select a character by its name (eg. "Petr Vectern").
/// </summary>
public class CharacterNameSelector : CharacterSelector
{
    /// <summary>
    /// The name of the character (eg. "Petr Vectern").
    /// </summary>
    public string CharacterName
    {
        get;
    }

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="characterName">
    /// The name of the character (eg. "Petr Vectern").
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
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
