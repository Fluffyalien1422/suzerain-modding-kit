using HarmonyLib;
using Il2Cpp;
using Il2CppPixelCrushers.DialogueSystem;
using MelonLoader;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation;

[HarmonyPatch(typeof(ConversationPanel), nameof(ConversationPanel.Setup))]
internal static class ConversationPanel_Setup_Patch
{
    // Use Prefix to modify the conversation before it starts.
    public static void Prefix(ConversationData conversationData)
    {
        Melon<Core>.Logger.Msg("ConversationPanel_Setup_Patch.Prefix");

        string title = conversationData.ConversationProperties.Dialogue;

        DialogueDatabase db = DialogueManager.MasterDatabase;
        DialogueConversation conversation = db.GetConversation(title);
        if (conversation == null)
        {
            Melon<Core>.Logger.Warning($"Conversation '{title}' not found in database.");
            return;
        }

        DialogueEntry parent = conversation.GetDialogueEntry(3);
        if (parent == null)
        {
            Melon<Core>.Logger.Warning($"Conversation '{title}' has no dialogue entries.");
            return;
        }
        Melon<Core>.Logger.Msg($"parent Title: {parent.Title}, ActorID: {parent.ActorID}, ConversantID: {parent.ConversantID}, DialogueText: {parent.DialogueText}, MenuText: {parent.MenuText}");
        //parent Title: Narrator: "Dark clouds were looming over Deyr....", ActorID: 10, ConversantID: 5, DialogueText: , MenuText:
        foreach (Link link in parent.outgoingLinks)
        {
            Melon<Core>.Logger.Msg($"parent.outgoingLinks Link from {link.originDialogueID} to {link.destinationDialogueID}");
        }
        foreach (Field field in parent.fields)
        {
            Melon<Core>.Logger.Msg($"parent.fields title: {field.title}, value: {field.value}, typeString: {field.typeString}");
        }

        Template template = Template.FromDefault();
        int newId = template.GetNextDialogueEntryID(conversation);

        DialogueEntry newEntry = template.CreateDialogueEntry(newId, conversation.id, string.Empty);
        newEntry.Title = "SuzerainModdingKit.Test";
        newEntry.ActorID = parent.ActorID;
        newEntry.ConversantID = parent.ConversantID;
        newEntry.currentLocalizedDialogueText = "Hi!";
        newEntry.currentLocalizedSequence = string.Empty;
        // Articy Id is required and used to track whether the dialogue entry has been read.
        // TODO: generate deterministic articy ids for modded dialogue entries, so that they can be tracked across saves.
        newEntry.SetTextField("Articy Id", "0x0FFFFFFF00000001");
        Melon<Core>.Logger.Msg(DialogueSkipHelper.HasBeenRead("0x0FFFFFFF00000001"));

        Melon<Core>.Logger.Msg($"newEntry Title: {newEntry.Title}, ActorID: {newEntry.ActorID}, ConversantID: {newEntry.ConversantID}, DialogueText: {newEntry.DialogueText}, MenuText: {newEntry.MenuText}");
        conversation.dialogueEntries.Add(newEntry);

        foreach (Field field in newEntry.fields)
        {
            Melon<Core>.Logger.Msg($"newEntry.fields title: {field.title}, value: {field.value}, typeString: {field.typeString}");
        }

        // There is also a Conversation.NewLink method, but it does not appear on the
        // Dialogue System docs, so avoid using it and just create the link manually.
        Link newLink = new(conversation.id, parent.id, conversation.id, newEntry.id)
        {
            priority = ConditionPriority.High,
        };
        parent.outgoingLinks.Add(newLink);

        Link newLink2 = new(conversation.id, newEntry.id, conversation.id, 126);
        newEntry.outgoingLinks.Add(newLink2);
    }
}
