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
    public int ParentID
    {
        get; init;
    }
    public int NextID
    {
        get; init;
    }
    public int? SpeakerID
    {
        get; init;
    }

    public bool IsChoice => SpeakerID == null;

    public ConversationNode(string name, string text, int parentID, int nextID, int? speakerID)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Text = text ?? throw new ArgumentNullException(nameof(text));
        ParentID = parentID;
        NextID = nextID;
        SpeakerID = speakerID;
    }
}
