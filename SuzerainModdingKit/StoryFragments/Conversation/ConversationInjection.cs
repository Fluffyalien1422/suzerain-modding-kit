namespace SuzerainModdingKit.StoryFragments.Conversation;

public class ConversationInjection
{
    private readonly List<ConversationNode> _nodes = [];
    public IReadOnlyList<ConversationNode> Nodes => _nodes;

    public string ConversationTitle
    {
        get; init;
    }

    public ConversationInjection(string conversationTitle)
    {
        ConversationTitle = conversationTitle ??
            throw new ArgumentNullException(nameof(conversationTitle));
    }

    public ConversationInjection AddNode(ConversationNode node)
    {
        ArgumentNullException.ThrowIfNull(node);
        _nodes.Add(node);
        return this;
    }
}
