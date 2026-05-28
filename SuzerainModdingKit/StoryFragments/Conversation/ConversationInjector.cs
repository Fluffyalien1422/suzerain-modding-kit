using System.Collections.ObjectModel;
using System.Globalization;
using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;
using SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;
using SuzerainModdingKit.Utils;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation;

internal static class ConversationInjector
{
    private static readonly HashSet<string> _conversationsPatched = [];

    private static bool CompareLink(Link link, Link other)
    {
        return link.destinationDialogueID == other.destinationDialogueID
            && link.destinationConversationID == other.destinationConversationID;
    }
    private static bool CompareLink(Link link, DialogueEntry other)
    {
        return link.destinationDialogueID == other.id
            && link.destinationConversationID == other.conversationID;
    }

    private static bool CompareEntry(DialogueEntry entry, DialogueEntry other)
    {
        return entry.id == other.id && entry.conversationID == other.conversationID;
    }

    private static List<DialogueEntry> FindFinalNodes(
        DialogueEntry originEntry,
        IEnumerable<Link> parentLinks)
    {
        Stack<DialogueEntry> stack = new();
        stack.Push(originEntry);
        List<DialogueEntry> finalNodes = [];
        List<Link> visited = [.. parentLinks];

        while (stack.Count > 0)
        {
            DialogueEntry entry = stack.Pop();
            if (entry.outgoingLinks.Count == 0)
            {
                finalNodes.Add(entry);
                continue;
            }
            foreach (Link link in entry.outgoingLinks)
            {
                // Check if we've already visited a link with the same destination.
                bool linkExists(Link other)
                {
                    return CompareLink(link, other);
                }
                if (visited.Exists(linkExists))
                {
                    // Ignore circular references.
                    continue;
                }
                visited.Add(link);

                // Get the destination entry.
                DialogueConversation conversation =
                    DialogueUtils.GetConversation(link.destinationConversationID);
                DialogueEntry nextEntry = conversation?.GetDialogueEntry(link.destinationDialogueID);
                if (nextEntry == null)
                {
                    // Ignore null references.
                    continue;
                }
                stack.Push(nextEntry);
            }
        }

        return finalNodes;
    }

    private static void HookNode(ResolvedConversationNodeHook resolvedHook)
    {
        ConversationNodeHook hook = resolvedHook.Hook;
        InjectedConversationNode node = resolvedHook.Node;
        DialogueEntry parent = resolvedHook.ResolvedParent;

        Link link = new(
            parent.conversationID, parent.id,
            node.Conversation.id, node.Entry.id)
        {
            priority = hook.ConditionPriority,
        };

        // The 'End' function ends the conversation (as you might've guessed) and is included
        // in all final nodes in the vanilla dialogues. Remove 'End' calls from the parent
        // so the new nodes will play.
        parent.userScript = parent.userScript
            .Replace("End()", string.Empty, StringComparison.Ordinal);
        // 'End' seems to not actually be required to end a conversation since Dialogue System
        // does it automatically if the last played node has no outgoing links, so we don't
        // need to add 'End' to our custom nodes.

        // ConditionGated: Choose the first (sorted by priority) outgoing link with a successful
        // condition. For choices, all with successful conditions will show.
        // This is the default behavior of Dialogue System, so just add it to the
        // outgoingLinks and let Dialogue System handle it.
        if (hook.Mode == ConversationNodeHook.HookMode.ConditionGated)
        {
            parent.outgoingLinks.Add(link);
            return;
        }

        // Override: Delete all other outgoing links and add this one.
        if (hook.Mode == ConversationNodeHook.HookMode.Override)
        {
            parent.outgoingLinks.Clear();
            parent.outgoingLinks.Add(link);
            return;
        }

        // Split: Break the chain at this point and insert this node in-between.

        List<DialogueEntry> finalNodes = FindFinalNodes(
            node.Entry,
            Il2CppUtils.ListFromIl2CppList(parent.outgoingLinks));

        // Copy the parent's outgoing links to each final entry.
        foreach (DialogueEntry finalEntry in finalNodes)
        {
            foreach (Link nextLink in parent.outgoingLinks)
            {
                finalEntry.outgoingLinks.Add(new Link(
                    finalEntry.conversationID, finalEntry.id,
                    nextLink.destinationConversationID, nextLink.destinationDialogueID));
            }
        }

        // Clear the parent's links and add this one, which should eventually link back
        // to the parent's original links.
        parent.outgoingLinks.Clear();
        parent.outgoingLinks.Add(link);
    }

    // CreateNodeOutgoingLinks explicitly takes ReadOnlyCollection rather than an interface
    // to ensure we never pass a mutable collection to selector.Resolve.
    private static void CreateNodeOutgoingLinks(
        InjectedConversationNode node,
        ReadOnlyCollection<InjectedConversationNode> nodes)
    {
        for (int i = 0; i < node.Node.NextNodes.Count; i++)
        {
            ConversationNodeSelector selector = node.Node.NextNodes[i];
            DialogueEntry entry = selector.Resolve(node.Conversation, nodes);
            if (entry == null)
            {
                Melon<Core>.Logger.Warning("Failed to resolve next node " +
                    $"{i.ToString(CultureInfo.InvariantCulture)} for conversation node " +
                    $"'{node.Node.Name}'.");
                continue;
            }

            Func<Link, bool> exists = (link) => CompareLink(link, entry);
            bool linkExists = node.Entry.outgoingLinks.Exists(exists);
            if (linkExists)
            {
                Melon<Core>.Logger.Warning("Found duplicate outgoing links " +
                    $"from conversation node '{node.Node.Name}'.");
                continue;
            }

            Link nextLink = new(
                node.Conversation.id, node.Entry.id,
                entry.conversationID, entry.id);
            node.Entry.outgoingLinks.Add(nextLink);
        }
    }

    // ResolveHooks explicitly takes ReadOnlyCollection rather than an interface
    // to ensure we never pass a mutable collection to selector.Resolve.
    private static List<ResolvedConversationNodeHook> ResolveHooks(
        ReadOnlyCollection<InjectedConversationNode> nodes)
    {
        List<ResolvedConversationNodeHook> resolvedHooks = [];
        foreach (InjectedConversationNode node in nodes)
        {
            List<DialogueEntry> resolvedParents = [];

            for (int i = 0; i < node.Node.Hooks.Count; i++)
            {
                ConversationNodeHook hook = node.Node.Hooks[i];
                ConversationNodeSelector selector = hook.Selector;
                DialogueEntry parentEntry = selector.Resolve(node.Conversation, nodes);
                if (parentEntry == null)
                {
                    Melon<Core>.Logger.Warning("Failed to resolve hook " +
                        $"{i.ToString(CultureInfo.InvariantCulture)} for conversation node " +
                        $"'{node.Node.Name}'.");
                    continue;
                }

                // Check if this node hooks to the same parent multiple times.
                bool parentExists(DialogueEntry entry)
                {
                    return CompareEntry(entry, parentEntry);
                }
                if (resolvedParents.Exists(parentExists))
                {
                    Melon<Core>.Logger.Warning($"Conversation node '{node.Node.Name}' has " +
                        "duplicate hooks.");
                    continue;
                }
                resolvedParents.Add(parentEntry);

                // Check if the parent already has a link to this node.
                Func<Link, bool> linkExists = (link) => CompareLink(link, node.Entry);
                bool doesLinkExist = parentEntry.outgoingLinks.Exists(linkExists);
                if (doesLinkExist)
                {
                    Melon<Core>.Logger.Warning("Found duplicate incoming links " +
                        $"to conversation node '{node.Node.Name}'.");
                    continue;
                }

                ResolvedConversationNodeHook resolved = new(hook, node, parentEntry);
                resolvedHooks.Add(resolved);
            }
        }
        return resolvedHooks;
    }

    private static void LinkInjectedNodes(ReadOnlyCollection<InjectedConversationNode> nodes)
    {
        // Create the outgoing links first. All the outgoing links have to be created before
        // we can create hooks.
        foreach (InjectedConversationNode node in nodes)
        {
            CreateNodeOutgoingLinks(node, nodes);
        }

        // Resolve and create hooks.
        List<ResolvedConversationNodeHook> resolvedHooks = ResolveHooks(nodes);
        resolvedHooks = [.. resolvedHooks.OrderBy(h => h.Hook.Priority)];
        foreach (ResolvedConversationNodeHook hook in resolvedHooks)
        {
            HookNode(hook);
        }
    }

    private static InjectedConversationNode InjectNode(
        ConversationNode node,
        DialogueConversation conversation)
    {
        int? speakerID = node.SpeakerSelector?.Resolve();
        if (speakerID == null && !node.IsChoice)
        {
            Melon<Core>.Logger.Error($"Failed to inject conversation node '{node.Name}': " +
                "Speaker character could not be resolved.");
            return null;
        }

        Template template = Template.FromDefault();
        int newID = template.GetNextDialogueEntryID(conversation);

        DialogueEntry newEntry = template.CreateDialogueEntry(newID, conversation.id, node.Name);
        string articyID = ArticyIDGenerator.GenerateArticyID(node.Name);
        newEntry.SetTextField("Articy Id", articyID);
        newEntry.currentLocalizedDialogueText = node.Text;
        newEntry.userScript = node.LuaScript;
        newEntry.conditionsString = node.LuaCondition;
        newEntry.currentLocalizedSequence = node.Sequence;

        // Actor = The person speaking the line.
        // Conversant = The person listening to the line.
        if (node.IsChoice)
        {
            // conversation.ActorID is the player.
            newEntry.ActorID = conversation.ActorID;
            // ConversantID doesn't matter for choices, so just inherit from the conversation.
            newEntry.ConversantID = conversation.ConversantID;
        }
        else
        {
            newEntry.ActorID = speakerID ?? 0;
            // The conversant should be the player.
            newEntry.ConversantID = conversation.ActorID;
        }

        conversation.dialogueEntries.Add(newEntry);

        return new InjectedConversationNode(node, newEntry, conversation);
    }

    public static void PatchConversation(DialogueConversation conversation)
    {
        if (_conversationsPatched.Contains(conversation.Title, StringComparer.Ordinal))
        {
            return;
        }
        _conversationsPatched.Add(conversation.Title);

        Melon<Core>.Logger.Msg($"Patching conversation '{conversation.Title}'.");

        List<InjectedConversationNode> injectedNodes = [];
        foreach (ConversationInjection injection in ConversationRegistry.Injections)
        {
            if (!injection.ConversationTitle.Equals(conversation.Title, StringComparison.Ordinal))
            {
                continue;
            }

            foreach (ConversationNode node in injection.Nodes)
            {
                InjectedConversationNode injected = InjectNode(node, conversation);
                if (injected != null)
                {
                    injectedNodes.Add(injected);
                }
            }
        }
        Melon<Core>.Logger.Msg($"Injected {injectedNodes.Count} nodes.");
        if (injectedNodes.Count == 0)
        {
            return;
        }

        ReadOnlyCollection<InjectedConversationNode> injectedNodesReadOnly = new(injectedNodes);
        LinkInjectedNodes(injectedNodesReadOnly);

        Melon<Core>.Logger.Msg($"Patched conversation '{conversation.Title}'.");
    }
}
