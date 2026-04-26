using HarmonyLib;
using Il2Cpp;
using Il2CppPixelCrushers.DialogueSystem;
using DialogueConversation = Il2CppPixelCrushers.DialogueSystem.Conversation;

namespace SuzerainModdingKit.StoryFragments.Conversation;

[HarmonyPatch(typeof(ConversationPanel), nameof(ConversationPanel.Setup))]
internal static class ConversationPanel_Setup_Patch
{
    // Use Prefix to modify the conversation before it starts.
    public static void Prefix(ConversationData conversationData)
    {
        string title = conversationData.ConversationProperties.Dialogue;

        DialogueDatabase db = DialogueManager.MasterDatabase;
        DialogueConversation conversation = db?.GetConversation(title);
        if (conversation == null)
        {
            return;
        }

        ConversationInjector.LoadInjections(conversation);
    }
}
