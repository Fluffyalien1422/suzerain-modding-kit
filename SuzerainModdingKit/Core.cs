using MelonLoader;
using SuzerainModdingKit.Save;
using SuzerainModdingKit.StoryFragments.Conversation;
using UnityEngine;
using UnityEngine.InputSystem;

[assembly: MelonInfo(typeof(SuzerainModdingKit.Core), "Suzerain Modding Kit", "0.1.0", "Fluffyalien", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]
// (also update version in ModdingKitConstants.cs)

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

        //TODO: automatically chain injections from multiple mods together
        // so multiple mods can hook to the same parent node.
        //TODO: node conditions and scripts

        //_ = ConversationRegistry.RegisterInjection(
        //    new ConversationInjection("Sordland/Turn02/Personal_Funeral")
        //        .AddNode(new ConversationNode(
        //            name: "TestMod.Hello",
        //            text: "Hello from Suzerain Modding Kit!",
        //            hookSelectors: [new ConversationNodeArticyIDSelector("0x0100000400008561")],
        //            // nextNodeSelectors is omitted because the nodes below are hooked to this one
        //            // so nextNodeSelectors is not necessary.
        //            speakerSelector: new CharacterNameSelector("Petr Vectern")))
        //        // if speakerSelector is omitted or set to null (speakerSelector: null)
        //        // then the node will be listed as a choice instead of a line of dialogue.
        //        // the following are choices because they omit speakerSelector.
        //        .AddNode(new ConversationNode(
        //            name: "TestMod.Hello2",
        //            text: "Hey Petr!",
        //            hookSelectors: [new ConversationNodeModdedNameSelector("TestMod.Hello")],
        //            nextNodeSelectors: [new ConversationNodeArticyIDSelector("0x01000003000545DC")]))
        //        .AddNode(new ConversationNode(
        //            name: "TestMod.Hello3",
        //            text: "Hello Petr.",
        //            hookSelectors: [new ConversationNodeModdedNameSelector("TestMod.Hello")],
        //            nextNodeSelectors: [new ConversationNodeArticyIDSelector("0x01000003000545DC")])));

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
