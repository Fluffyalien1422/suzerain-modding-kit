# Contributing

## Setup

1. Install Visual Studio.
2. Follow the instructions on the [MelonLoader Wiki](https://melonwiki.xyz/#/README) to download and install MelonLoader.
3. Install a .NET decompiler like [dotPeek](https://www.jetbrains.com/decompiler/) or similar program of your choice.
4. Launch Suzerain so MelonLoader can create the game assembly. Launching the game normally from Steam should launch the MelonLoader terminal before launching the game. Ensure everything runs successfully.
5. Open `C:\Program Files (x86)\Steam\steamapps\common\Suzerain\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll` in the decompiler to view Suzerain's types. Suzerain types are under the `Il2Cpp` namespace (the other namespaces are Suzerain's dependencies).
6. Fork and clone this repository and open it in Visual Studio.
7. Build the solution and it will automatically be copied to the `Mods` folder (ensure Suzerain is closed). Launch Suzerain to test.

