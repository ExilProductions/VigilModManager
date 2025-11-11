# VigilModManager

A MelonLoader mod manager for the game Vigil that provides an in-game interface for managing loaded mods and their settings.

## Technical Details

The mod is built using Melonloader 7.1


## Installation

1. Install MelonLoader for Vigil if not already installed
2. Place `VigilModManager.dll` in the `Mods` folder of your Vigil installation
3. Launch the game - the mod will automatically initialize and add a "Mods" button to the main menu

## Usage

### For Players

1. Launch Vigil and navigate to the main menu
2. Click the "Mods" button to open the mod management interface
3. Browse the list of installed mods on the left
4. Click on any mod to view its information on the right
5. If a mod has configurable settings, click "Settings" to open its configuration panel

### For Mod Developers

To add settings to your mod:

```csharp
using VMM.ModRegistry.Settings;
using VMM.ModRegistry.Settings.Types;

// Create settings container
var settings = new ModSettings();

// Add a toggle setting
var toggleSetting = new ToggleSetting
{
    Name = "Enable Feature",
    Value = true,
    OnChanged = (value) => {
        // Handle setting change
        YourMod.HandleToggleChange(value);
    }
};
settings.AddSetting(toggleSetting);

// Add a slider setting
var sliderSetting = new SliderSetting
{
    Name = "Intensity",
    Value = 50f,
    MinValue = 0f,
    MaxValue = 100f,
    OnChanged = (value) => {
        // Handle slider change
        YourMod.HandleSliderChange(value);
    }
};
settings.AddSetting(sliderSetting);

// Register settings with the manager
VMM.Core.ModManager.Instance.RegisterSettings(Assembly.GetExecutingAssembly(), settings);
```


## Development

### Building

1. Set the `VIGIL_GAME_DIR` environment variable to your Vigil installation directory
2. Build the project using Visual Studio or `dotnet build`
3. The post-build event automatically copies the compiled DLL to the game's Mods folder

## License

This project is licensed under the Apache License 2.0. See LICENSE.txt for the full license text.