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
    public ConversationNodeSelector ParentNodeSelector
    {
        get; init;
    }
    public ConversationNodeSelector NextNodeSelector
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
        ConversationNodeSelector parentNodeSelector,
        ConversationNodeSelector nextNodeSelector,
        CharacterSelector speakerSelector = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Text = text ?? throw new ArgumentNullException(nameof(text));
        ParentNodeSelector = parentNodeSelector
            ?? throw new ArgumentNullException(nameof(parentNodeSelector));
        NextNodeSelector = nextNodeSelector
            ?? throw new ArgumentNullException(nameof(nextNodeSelector));
        SpeakerSelector = speakerSelector;
    }
}
