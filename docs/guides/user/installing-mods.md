# Installing Mods

A beginner's guide to installing Suzerain mods.

You must own Suzerain **on Steam** and you must have it installed. This guide covers Windows (native) and Linux (via Steam Proton). See the section for your OS below.

## Understand the Risks

Mods are third-party code that run directly on your computer with the same permissions as the game itself. This means a malicious mod can cause serious harm.

**Only ever install mods from sources you trust. Do not download or install anything you are unsure about.**

We only distribute Suzerain Modding Kit on the [official repository](https://github.com/suzerain-modding/suzerain-modding-kit) and the official Nexus Mods page (TODO: link nexus mods post). **Do not download Suzerain Modding Kit from any other source.**

### Beta Disclaimer

Suzerain Modding Kit is currently in beta and should not be considered stable. Expect bugs and crashes.

## Back Up Saves

If something goes wrong, your save files could get lost or corrupted. See [Back Up Saves](back-up-saves.md) for back up instructions.

# Windows

## Install .NET and Visual C++ Redistributable

To run mods for Suzerain, you must have the following installed on your computer. The exact versions linked here are required.

- [Microsoft Visual C++ 2015-2019 Redistributable 64 Bit](https://aka.ms/vs/16/release/vc_redist.x64.exe).
- [.NET Desktop Runtime 6.0.36](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-6.0.36-windows-x64-installer).

## Install MelonLoader

MelonLoader is the program that will inject the mods into Suzerain.

Install [MelonLoader.Installer.exe](https://github.com/LavaGang/MelonLoader.Installer/releases/latest/download/MelonLoader.Installer.exe). Launch the installer, then select Suzerain.

**I want to install MelonLoader manually (I know what I am doing!):** See the [MelonLoader Wiki](https://melonwiki.xyz/#/README?id=manual-installation) for manual installation instructions.

## Launch Suzerain

You must launch Suzerain after installing MelonLoader atleast once so MelonLoader can create the necessary files.

Launch Suzerain from Steam as normal. This will launch the MelonLoader terminal before launching the game. The first launch may take a while. Once you reach the Suzerain main menu screen, just exit the game.

## Install Suzerain Modding Kit

Suzerain Modding Kit is the modding API for Suzerain.

To install Suzerain Modding Kit:

1. Download the latest version (TODO: add download link).
2. Launch Steam if it's not already open.
3. Select Suzerain in your library.
4. Select the gear icon > Manage > Browse local files.
5. Move the Suzerain Modding Kit DLL into the `Mods` folder.
6. Check if Suzerain Modding Kit is working:
    1. Launch Suzerain.
    2. Once you're in the main menu screen, press Ctrl + D. If Suzerain Modding Kit is working, you'll see a debug overlay appear. Press Ctrl + D again or press the "Hide" button in the top right to close the debug menu.
        - Don't worry if it says "GameFlowManager not loaded!" This just means that there is no loaded campaign, which is because you are in the main menu.
    3. Quit the game.

## Install Mods

Remember to **only ever install mods from sources you trust!**

If a mod provides its own installation instructions, follow them. Otherwise, follow the instructions below.

To install most mods:

1. Download the DLL and its required dependencies if it has any.
2. Launch Steam if it's not already open.
3. Select Suzerain in your library.
4. Select the gear icon > Manage > Browse local files.
5. Move the DLL(s) into the `Mods` folder.

# Linux (Steam Proton)

Suzerain on Linux runs as a Windows binary inside Proton's Wine prefix. MelonLoader and Suzerain Modding Kit run inside that same prefix, so they behave exactly as they would on Windows — you just install them slightly differently.

## Install MelonLoader (Linux)

MelonLoader doesn't have a Linux-native installer. Pick one of the two approaches below.

### Option 1: Manual install (recommended for Linux)

This is the simplest and most reliable path on Linux.

1. Download the latest MelonLoader release zip from the [MelonLoader Releases page](https://github.com/LavaGang/MelonLoader/releases/latest). Pick the `MelonLoader.x64.zip` asset (Suzerain is 64-bit).
2. Extract the contents directly into your Suzerain folder. To find the folder:
    1. Launch Steam.
    2. Select Suzerain in your library.
    3. Select the gear icon > Manage > Browse local files.
    4. Steam will open the Suzerain folder in your file manager. The path is usually `~/.steam/steam/steamapps/common/Suzerain` (or `~/.steam/debian-installation/steamapps/common/Suzerain` on Debian/Ubuntu, or `~/.var/app/com.valvesoftware.Steam/data/Steam/steamapps/common/Suzerain` for Flatpak Steam).
3. After extraction, the Suzerain folder should contain a new `MelonLoader` folder and a `version.dll` file next to `Suzerain.exe`.

### Option 2: Run the Windows installer through Proton

If you'd rather use the GUI installer, you can run it under Proton:

1. Download [MelonLoader.Installer.exe](https://github.com/LavaGang/MelonLoader.Installer/releases/latest/download/MelonLoader.Installer.exe).
2. Right-click the file in your file manager and select "Open with → Steam" (or run `steam steam://rungameid/0/run/$(realpath MelonLoader.Installer.exe)`). If Steam doesn't pick it up automatically, you can add it as a non-Steam game with Proton selected as the compatibility tool.
3. In the installer, point it at the Suzerain folder (same paths as in Option 1).

## Launch Suzerain

You must launch Suzerain after installing MelonLoader at least once so MelonLoader can create the necessary files.

Launch Suzerain from Steam as normal. MelonLoader's terminal output will appear in the Steam launch console or in `~/.steam/.../Suzerain/MelonLoader/Latest.log`. The first launch may take a while. Once you reach the Suzerain main menu, exit the game.

## Install Suzerain Modding Kit (Linux)

1. Download the latest Suzerain Modding Kit release (TODO: add download link).
2. Open your Suzerain folder via Steam > gear icon > Manage > Browse local files.
3. Move the Suzerain Modding Kit DLL into the `Mods` folder.
4. Verify it loads:
    1. Launch Suzerain from Steam.
    2. From the main menu, press Ctrl + D. If you see the debug overlay, you're set. Press Ctrl + D again to close it.
        - "GameFlowManager not loaded!" is expected at the main menu — it just means no campaign is active.
    3. Quit the game.

## Install other mods (Linux)

Same as on Windows: drop the mod DLL into your Suzerain `Mods/` folder. Always treat mod DLLs as third-party code and only install from sources you trust.

# Disabling or Uninstalling Mods

See [Disabling Mods](disabling-mods.md) to learn how to disable mods without uninstalling them.

See [Uninstalling Mods](uninstalling-mods.md) to learn how to uninstall mods or uninstall MelonLoader entirely.

