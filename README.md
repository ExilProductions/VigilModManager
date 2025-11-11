# VigilModManager

A MelonLoader mod manager for the game Vigil that provides an in-game interface for managing loaded mods and their settings.

## Overview

VigilModManager is a mod loading framework that adds a "Mods" button to the main menu of Vigil. It automatically detects all loaded MelonLoader mods and provides a user-friendly interface to view mod information and configure mod settings through a unified settings system.

## Features

- **Automatic Mod Detection**: Scans and lists all loaded MelonLoader mods with their name, version, and author
- **In-Game UI**: Integrates seamlessly into Vigil's main menu with custom tabs for mod management
- **Settings Framework**: Provides a standardized system for mods to register configurable settings
- **Setting Types**: Supports toggle switches and slider controls (extensible for more types)
- **Mod Information Display**: Shows detailed information about each loaded mod
- **Real-time Configuration**: Settings changes take effect immediately without restarting the game

## Technical Details

### Architecture

The mod is built using:
- **MelonLoader**: Modding framework for Unity games
- **Harmony**: Runtime patching library for modifying game methods
- **.NET Standard 2.1**: Target framework for compatibility

### Project Structure

```
VigilModManager/
├── Core.cs                    # Main mod entry point and initialization
├── ModRegistry/               # Mod management system
│   ├── ModEntry.cs           # Data structure for mod information
│   ├── ModManager.cs         # Central mod registry and loader
│   └── Settings/             # Settings framework
│       ├── ModSettings.cs    # Settings container for mods
│       ├── ISettingsElement.cs # Interface for setting elements
│       ├── SettingsElement.cs # Base class for settings
│       └── Types/            # Specific setting implementations
│           ├── ToggleSetting.cs
│           └── SliderSetting.cs
├── Patches/                   # Game method patches
│   └── MainMenuManager.cs   # Harmony patches for main menu access
└── UI/                       # User interface components
    ├── UIManager.cs          # Main UI controller
    ├── Components/           # UI component scripts
    │   ├── ModEntryButton.cs
    │   ├── ModInfoDisplayer.cs
    │   ├── ModSettingsManager.cs
    │   └── UILayoutRebuilder.cs
    └── Helpers/              # UI building utilities
        └── UIBuilder.cs
```

### Key Components

- **Core**: Main MelonMod class that initializes the manager systems
- **ModManager**: Singleton that tracks all loaded mods and their settings
- **UIManager**: Handles creation and management of the in-game interface
- **Settings System**: Framework for mods to register configurable options

## Installation

1. Install MelonLoader for Vigil if not already installed
2. Place `VigilModManagerML.dll` in the `Mods` folder of your Vigil installation
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

## Configuration

The mod automatically detects and loads all MelonLoader mods. No additional configuration is required for basic functionality.

## Compatibility

- **Game**: Vigil (by Singularity Studios)
- **Framework**: MelonLoader
- **.NET Version**: .NET Standard 2.1
- **Unity Version**: Compatible with Vigil's Unity runtime

## Development

### Building

1. Set the `VIGIL_GAME_DIR` environment variable to your Vigil installation directory
2. Build the project using Visual Studio or `dotnet build`
3. The post-build event automatically copies the compiled DLL to the game's Mods folder

### Dependencies

The project references Vigil's managed assemblies and MelonLoader libraries:
- All Unity engine assemblies used by Vigil
- MelonLoader.dll and 0Harmony.dll
- Game-specific assemblies (Assembly-CSharp, etc.)

## License

This project is licensed under the Apache License 2.0. See LICENSE.txt for the full license text.

## Contributing

Contributions are welcome. Please ensure:
- Code follows the existing style and conventions
- New features include appropriate error handling
- UI components integrate properly with the existing interface
- All changes are tested in-game

## Troubleshooting

### Common Issues

- **Mods button not appearing**: Ensure MelonLoader is properly installed and VMM is loading correctly
- **Settings not saving**: Settings are runtime-only and reset on game restart by design
- **UI layout issues**: Make sure you're in the main menu when accessing the mods interface

### Debug Information

The mod logs detailed information to the MelonLoader console, including:
- Initialization status
- Detected mods with their information
- Settings registration confirmations
- UI creation and error messages

Check the MelonLoader console for diagnostic information if issues occur.