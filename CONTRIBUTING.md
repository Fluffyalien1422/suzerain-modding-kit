# Contributing

## Coding Guidelines and Info

Please read and follow the [coding guidelines](CODING_GUIDELINES.md) when contributing to Suzerain Modding Kit.

Reading the [Suzerain Implementation Info](suzerain_implementation_info.md) document will also be helpful in understanding how Suzerain works.

## Basic Setup

This guide is intended for the Steam version of Suzerain. Pick the section for your OS — both produce the same DLL.

### On Windows

1. Install Visual Studio.
2. Follow the instructions on the [MelonLoader Wiki](https://melonwiki.xyz/#/README) to download and install MelonLoader.
3. Install a .NET decompiler like [dotPeek](https://www.jetbrains.com/decompiler/) or similar program of your choice. Note that the assemblies that MelonLoader generates only contain the type names (class names, method names, etc), not the actual code. In most cases, this is sufficient. If actual code is required, see the [Decompiling with Ghidra](#decompiling-with-ghidra) section.
4. Launch Suzerain so MelonLoader can create the game assembly. Launching the game normally from Steam should launch the MelonLoader terminal before launching the game. Ensure everything runs successfully. Close the game before continuing.
5. Open `C:\Program Files (x86)\Steam\steamapps\common\Suzerain\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll` in the decompiler to view Suzerain's types. Suzerain types are under the `Il2Cpp` namespace (the other namespaces are Suzerain's dependencies).
6. Fork and clone this repository and open it in Visual Studio.
7. Build the solution and it will automatically be copied to the `Mods` folder (ensure Suzerain is closed). Launch Suzerain to test.

### On Linux (Steam Proton)

Suzerain itself is a Windows binary running under Proton, but the build toolchain is fully native — no Wine required for development.

1. Install the .NET 8 SDK (any 8.x or newer works; the project targets `net6.0` so any modern SDK can build it).
    - Debian/Ubuntu: `sudo apt install dotnet-sdk-8.0`
    - Fedora: `sudo dnf install dotnet-sdk-8.0`
    - Arch: `sudo pacman -S dotnet-sdk`
2. Install MelonLoader **inside the Suzerain prefix**. The official `MelonLoader.Installer.exe` works under Proton — right-click Suzerain in Steam → Properties → set a launch option of `%command%` and run the installer once via Steam, or follow the [manual install instructions](https://melonwiki.xyz/#/README?id=manual-installation) and extract MelonLoader into the Suzerain folder directly.
3. Install a .NET decompiler. Cross-platform options:
    - [ILSpy CLI (`ilspycmd`)](https://github.com/icsharpcode/ILSpy?tab=readme-ov-file#ilspycmd) — `dotnet tool install -g ilspycmd`
    - [AvaloniaILSpy](https://github.com/icsharpcode/AvaloniaILSpy) — the cross-platform ILSpy GUI
    - JetBrains Rider has dotPeek's decompiler built in
4. Launch Suzerain through Steam at least once so MelonLoader can generate `MelonLoader/Il2CppAssemblies/`. Then close it.
5. Open `~/.steam/steam/steamapps/common/Suzerain/MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll` in your decompiler. Suzerain types are under the `Il2Cpp` namespace.
6. Fork and clone this repository.
7. Build: `dotnet build SuzerainModdingKit/SuzerainModdingKit.csproj -c Debug -p:Platform=x64`. The DLL will be copied automatically to your Suzerain `Mods` folder.
    - If your Steam library lives somewhere non-default, override the path with `SUZERAIN_GAME_PATH=/your/path dotnet build ...` or create a `Directory.Build.user.props` next to `Directory.Build.props` (gitignored) that sets `<GamePath>`.

### IDE choices on Linux

Any of these work; pick whichever you prefer:

- **VS Code** with the [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) extension
- **JetBrains Rider** (commercial; free for non-commercial use)
- **Neovim/Helix** with `omnisharp-roslyn` or `csharp-ls`

## Decompiling with Ghidra

The standard decompiling method explained above only allows you to see type names. This should be sufficient in most cases, but we can also decompile the actual code using Ghidra.

See [this gist](https://gist.github.com/BadMagic100/47096cbcf64ec0509cf75d48cfbdaea5) which explains how to decompile IL2CPP games (like Suzerain) with Ghidra. **You must make the following changes to the guide to use it in newer versions of Ghidra:**

- The guide tells you to use OpenJDK 17, but newer versions of Ghidra require a different version. See [the latest requirements](https://github.com/NationalSecurityAgency/ghidra/blob/master/GhidraDocs/GettingStarted.md#minimum-requirements).
- Add `# @runtime Jython` to the first line of `ghidra_with_struct.py` before running it in Ghidra.
