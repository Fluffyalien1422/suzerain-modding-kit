using System.Collections.ObjectModel;
using SuzerainModdingKit.Character;
using SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;
using SuzerainModdingKit.Utils;

namespace SuzerainModdingKit.StoryFragments.Conversation;

/// <summary>
/// A dialogue line or choice in a conversation.
/// </summary>
public class ConversationNode
{
    /// <summary>
    /// The unique identifier of the node.
    /// </summary>
    public string Name
    {
        get;
    }
    /// <summary>
    /// The text of the node.
    /// </summary>
    public string Text
    {
        get;
    }
    /// <summary>
    /// A read-only list of hooks.
    /// </summary>
    /// <remarks>
    /// Hooks are nodes that this node should attach (or hook) to.
    /// </remarks>
    public ReadOnlyCollection<ConversationNodeHook> Hooks
    {
        get;
    }
    /// <summary>
    /// A read-only list of next node selectors.
    /// </summary>
    /// <remarks>
    /// These are the nodes that will show after this one.
    /// Only the first node with a successful condition will show. If the nodes are choices,
    /// all choice nodes with successful conditions will show.
    /// </remarks>
    public ReadOnlyCollection<ConversationNodeSelector> NextNodes
    {
        get;
    }
    /// <summary>
    /// The speaker of the line. This property is optional. If null, the node should
    /// be considered a choice rather than a dialogue line.
    /// </summary>
    public CharacterSelector SpeakerSelector
    {
        get;
    }
    /// <summary>
    /// An optional Lua script to run when the dialogue is spoken.
    /// </summary>
    public string LuaScript
    {
        get;
    }
    /// <summary>
    /// An optional Lua condition to determine whether the dialogue should be spoken or not.
    /// </summary>
    public string LuaCondition
    {
        get;
    }
    /// <summary>
    /// Optional conversation-related actions to perform when this dialogue is spoken.
    /// </summary>
    /// <seealso cref="ConversationNodeSequenceBuilder"/>
    public string Sequence
    {
        get;
    }
    /// <summary>
    /// Returns a boolean indicating whether the node should be considered a choice
    /// (is <c cref="SpeakerSelector">SpeakerSelector</c> null?).
    /// </summary>
    public bool IsChoice => SpeakerSelector == null;

    /// <summary>
    /// Creates a new instance of this class.
    /// </summary>
    /// <param name="name">
    /// The unique identifier of the node.
    /// </param>
    /// <param name="text">
    /// The text of the node.
    /// </param>
    /// <param name="hooks">
    /// Optional: A list of hooks. Hooks are nodes that this node should attach (or hook) to.
    /// Hooks should only be used when trying to link to a node that you don't control
    /// (such as a node from vanilla Suzerain or another mod). If you control the node you are
    /// trying to hook to, you should add this node to the other node's
    /// <c cref="NextNodes">NextNodes</c> instead.
    /// </param>
    /// <param name="nextNodes">
    /// Optional: A list of next node selectors. These are the nodes that will show after this one.
    /// Only the first node with a successful condition will show. If the nodes are choices,
    /// all choice nodes with successful conditions will show.
    /// </param>
    /// <param name="speakerSelector">
    /// Optional: The speaker of the line. If null, the node will
    /// be considered a choice rather than a dialogue line.
    /// </param>
    /// <param name="luaScript">
    /// Optional: A Lua script to run when the dialogue is spoken.
    /// </param>
    /// <param name="luaCondition">
    /// Optional: A Lua condition to determine whether the dialogue should be spoken or not.
    /// </param>
    /// <param name="sequence">
    /// Optional: Conversation-related actions to perform when this dialogue is spoken.
    /// See <see cref="ConversationNodeSequenceBuilder"/>.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if any required arguments are null.
    /// </exception>
    public ConversationNode(
        string name,
        string text,
        IReadOnlyList<ConversationNodeHook> hooks = null,
        IReadOnlyList<ConversationNodeSelector> nextNodes = null,
        CharacterSelector speakerSelector = null,
        string luaScript = null,
        string luaCondition = null,
        string sequence = null)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Text = text ?? throw new ArgumentNullException(nameof(text));
        Hooks = new ReadOnlyCollection<ConversationNodeHook>(hooks != null ? [.. hooks] : []);
        NextNodes = new ReadOnlyCollection<ConversationNodeSelector>(
            nextNodes != null ? [.. nextNodes] : []);
        SpeakerSelector = speakerSelector;
        LuaScript = luaScript;
        LuaCondition = luaCondition;
        Sequence = sequence;
    }
}
