using MelonLoader;
using SuzerainModdingKit.Save;
using SuzerainModdingKit.StoryFragments.Conversation;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(SuzerainModdingKit.Core), "Suzerain Modding Kit", "0.1.0", "Fluffyalien", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]
// (also update version in SmkInfo.cs)

namespace SuzerainModdingKit;

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg(
            $"Suzerain Modding Kit version: {SmkInfo.ModVersion}, " +
            $"Suzerain version: {Application.version}, " +
            $"Target Suzerain version: {SmkInfo.TargetSuzerainVersion}.");

        if (!string.Equals(
            Application.version,
            SmkInfo.TargetSuzerainVersion,
            StringComparison.Ordinal))
        {
            LoggerInstance.Warning(
                $"Expected Suzerain version {SmkInfo.TargetSuzerainVersion}, " +
                $"but got {Application.version}. Suzerain Modding Kit may not work properly.");
        }

        LoggerInstance.Msg("Cleaning up mod saves.");
        SaveManager.CleanupOrphanedModSaves();

        //new ConversationInjection("Sordland/Turn02/Personal_Funeral")
        //    .AddNode(new ConversationNode(
        //        name: "TestMod.Hello",
        //        text: "Hello from Suzerain Modding Kit!",
        //        speakerSelector: new CharacterNameSelector("Petr Vectern"),
        //        sequence: new ConversationNodeSequenceBuilder()
        //            .AddConversant("Iosef Lancea")
        //            .ToString(),
        //        luaScript: "Variable[\"TestMod.HelloNode\"]=true;",
        //        hooks: [
        //            new ConversationNodeHook(
        //                    selector: new ConversationNodeArticyIDSelector("0x0100000400008561")),
        //        ],
        //        nextNodes: [
        //            new ConversationNodeModdedNameSelector("TestMod.Hello2"),
        //            new ConversationNodeModdedNameSelector("TestMod.Hello3"),
        //        ]))
        //    .AddNode(new ConversationNode(
        //        name: "TestMod.Hello2",
        //        text: "Var is true.",
        //        luaCondition: "Variable[\"TestMod.HelloNode\"]==true"))
        //    .AddNode(new ConversationNode(
        //        name: "TestMod.Hello3",
        //        text: "Var is false.",
        //        luaCondition: "Variable[\"TestMod.HelloNode\"]==false"))
        //    .Register();

        LoggerInstance.Msg("Pre-initialization complete.");
    }

    public override void OnLateInitializeMelon()
    {
        ConversationRegistry.CloseRegistration();
        LoggerInstance.Msg("Conversation registration closed.");

        LoggerInstance.Msg("Initialized.");
    }

    public override void OnUpdate()
    {
        Keyboard kb = Keyboard.current;
        if (kb == null)
        {
            return;
        }

        if (kb.ctrlKey.isPressed && kb.dKey.wasPressedThisFrame)
        {
            DebugOverlay.SetVisibility(value: !DebugOverlay.IsShowing());
        }
    }

    public override void OnGUI()
    {
        DebugOverlay.Update();
    }
}
