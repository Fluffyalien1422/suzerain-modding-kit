namespace SuzerainModdingKit.Character;

/// <summary>
/// An abstract class that selects a character in the game.
/// </summary>
public abstract class CharacterSelector
{
    /// <summary>
    /// Select the character.
    /// </summary>
    /// <returns>
    /// The Dialogue System ID of the character or null if it could not be found.
    /// </returns>
    public abstract int? Resolve();
}
