using SuzerainModdingKit.Character;
using SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;

namespace SuzerainModdingKit.StoryFragments.Conversation;

public class ConversationNode
{
    public string Name
    {
        get; init;
    }
    public string Text
    {
        get; init;
    }
    public IReadOnlyList<ConversationNodeHook> Hooks
    {
        get; init;
    }
    public IReadOnlyList<ConversationNodeSelector> NextNodes
    {
        get; init;
    }
    public CharacterSelector SpeakerSelector
    {
        get; init;
    }

    public bool IsChoice => SpeakerSelector == null;

    public ConversationNode(
        string name,
        string text,
        IReadOnlyList<ConversationNodeHook> hooks = null,
        IReadOnlyList<ConversationNodeSelector> nextNodes = null,
        CharacterSelector speakerSelector = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Text = text ?? throw new ArgumentNullException(nameof(text));
        Hooks = hooks != null
            ? new List<ConversationNodeHook>(hooks) : [];
        NextNodes = nextNodes != null
            ? new List<ConversationNodeSelector>(nextNodes) : [];
        SpeakerSelector = speakerSelector;
    }
}
