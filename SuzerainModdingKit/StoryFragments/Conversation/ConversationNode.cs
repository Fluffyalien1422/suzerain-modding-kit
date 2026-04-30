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
    public IReadOnlyList<ConversationNodeSelector> HookSelectors
    {
        get; init;
    }
    public IReadOnlyList<ConversationNodeSelector> NextNodeSelectors
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
        IReadOnlyList<ConversationNodeSelector> hookSelectors = null,
        IReadOnlyList<ConversationNodeSelector> nextNodeSelectors = null,
        CharacterSelector speakerSelector = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Text = text ?? throw new ArgumentNullException(nameof(text));
        HookSelectors = hookSelectors != null
            ? new List<ConversationNodeSelector>(hookSelectors) : [];
        NextNodeSelectors = nextNodeSelectors != null
            ? new List<ConversationNodeSelector>(nextNodeSelectors) : [];
        SpeakerSelector = speakerSelector;
    }
}
