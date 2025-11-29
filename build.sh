#!/usr/bin/env bash

# VigilModManager build script for Linux
echo "Building VigilModManager..."

# Check if VIGIL_GAME_DIR is set
if [ -z "$VIGIL_GAME_DIR" ]; then
    echo "ERROR: VIGIL_GAME_DIR environment variable is not set!"
    echo "Please set VIGIL_GAME_DIR to your Vigil game installation directory."
    echo "Example: export VIGIL_GAME_DIR=\$HOME/.local/share/Steam/steamapps/common/Vigil"
    exit 1
fi

echo "Using Vigil game directory: $VIGIL_GAME_DIR"

# Build the project
dotnet build VMM.csproj --configuration Release
BUILD_EXIT_CODE=$?

if [ $BUILD_EXIT_CODE -eq 0 ]; then
    echo
    echo "Build completed successfully!"
    MOD_DIR="$VIGIL_GAME_DIR/Mods"
    mkdir -p "$MOD_DIR"
    echo "The mod DLL has been copied to: $MOD_DIR"
else
    echo
    echo "Build failed! Please check the error messages above."
    exit 1
fi

read -rp "Press Enter to exit..."
