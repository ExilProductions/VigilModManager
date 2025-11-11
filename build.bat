@echo off
echo Building VigilModManager...


if "%VIGIL_GAME_DIR%"=="" (
    echo ERROR: VIGIL_GAME_DIR environment variable is not set!
    echo Please set VIGIL_GAME_DIR to your Vigil game installation directory.
    echo Example: set VIGIL_GAME_DIR=C:\Games\Vigil
    pause
    exit /b 1
)

echo Using Vigil game directory: %VIGIL_GAME_DIR%

dotnet build VMM.csproj --configuration Release

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build completed successfully!
    echo The mod DLL has been copied to: %VIGIL_GAME_DIR%\Mods\
) else (
    echo.
    echo Build failed! Please check the error messages above.
    pause
    exit /b 1
)

pause