## Description and Features

**ZoomLevel** is a [Stardew Valley](http://stardewvalley.net/) mod based on [this mod](https://github.com/GuiNoya/SVMods/) and it allows you to change and adjust the zoom level and UI levels of the game.

- You can increase zoom level with `, (comma)` and decrease it with `. (period)`.<br>
  By holding `Left Shift` or `Right Shift` and using the controls above, you can change the UI Scale.

- If you use a controller, you can also adjust it by pressing the `left stick` to decrease the zoom and `right stick` to increases the zoom.<br>
  By holding `Left Trigger & Right Trigger` and using the controls above, you can change the UI Scale.

## Contents

- [Description and Features](#description-and-features)
- [Contents](#contents)
- [Installation](#installation)
- [Console Commands](#console-commands)
  - [In-game settings](#in-game-settings)
- [Configuration](#configuration)
  - [In-game settings](#in-game-settings-1)
  - [`config.json` file](#configjson-file)
- [Compatibility](#compatibility)
- [Changelog](#changelog)
- [See also](#see-also)

## Installation

1. [Install the latest version of SMAPI](https://smapi.io/).
2. Download the mod from [nexus mods](https://www.nexusmods.com/stardewvalley/mods/7363?tab=files) or from [github](https://github.com/thespbgamer/ZoomLevel/releases/).
3. Unzip the mod folder into your Stardew Valley/Mods.
4. Run the game using SMAPI.

## Console Commands

### In-game settings

Once you have the game running (having a save loaded), you can use any of these commands at any time.

Here's what you can do:

| Command Name                            | Optional Parameter(s)                                           | Description                                                                                                                |
| :-------------------------------------- | :-------------------------------------------------------------- | :------------------------------------------------------------------------------------------------------------------------- |
| `toggle_Auto_Zoom_Map`                  | `null` aka **nothing**                                          | Toggles the 'AutoZoomMap'.                                                                                                 |
| `toggle_Press_Any_Key_To_Reset_Camera`  | `null` aka **nothing**                                          | Toggles the 'PressAnyKeyToResetCamera'.                                                                                    |
| `toggle_Hide_With_UI_With_Certain_Zoom` | `null` aka **nothing**                                          | Toggles the 'HideWithUIWithCertainZoom'.                                                                                   |
| `toggle_Preset_On_Load_Save_File`       | `null` aka **nothing**                                          | Toggles the 'PresetOnLoadSaveFile'.                                                                                        |
| `reset_UI_and_Zoom`                     | `UIScale and zoomLevel` example **reset_UI_and_Zoom 0.90 0.75** | Resets the zoom and UI levels back to the default game values. Optional parameters are UI and Zoom values (in that order). |
| `reset_UI`                              | `UIScale` example **reset_UI 0.90**                             | Resets the UI scale. Optional parameter is the UI Scale value.                                                             |
| `reset_Zoom`                            | `zoomLevel` example **reset_Zoom 0.75**                         | Resets the Zoom Level. Optional parameter is the Zoom Level value.                                                         |

## Configuration

### In-game settings

If you have the [generic mod config menu](https://www.nexusmods.com/stardewvalley/mods/5098?tab=files) installed, the configuration process becomes much simpler, you can click the cog button (⚙) on the title screen or the "mod options" button at the bottom of
the in-game menu to configure the mod.

### `config.json` file

The mod creates a `config.json` file in its mod folder the first time you run it. You can open the file in a text editor like notepad to configure the mod.

Here's what you can change:

- Player controls:

  | Setting Name                                | Default Value                                                  | Description                                                           |
  | :------------------------------------------ | :------------------------------------------------------------- | :-------------------------------------------------------------------- |
  | `KeybindListHoldToChangeUI`                 | `LeftShift` or `RightShift` or `LeftTrigger and RightTrigger"` | Key you need to hold to change the UI.                                |
  | `KeybindListIncreaseZoomOrUI`               | `OemPeriod` aka `.` or `RightStick`                            | Key to Increase Zoom or UI Level.                                     |
  | `KeybindListDecreaseZoomOrUI`               | `OemComma` aka `,` or `LeftStick`                              | Key to Decrease Zoom or UI Level.                                     |
  | `KeybindListResetZoomOrUI`                  | `null` aka **nothing**                                         | Key to Reset the Zoom or UI Level.                                    |
  | `KeybindListMaxZoomOrUI`                    | `null` aka **nothing**                                         | Key to Max the Zoom out or Maximize the UI.                           |
  | `KeybindListMinZoomOrUI`                    | `null` aka **nothing**                                         | Key to Max the Zoom in or Minimize the UI.                            |
  | `KeybindListZoomToCurrentMapSize`           | `null` aka **nothing**                                         | Keybinds to change to zoom level to the approximate current map size. |
  | `KeybindListPresetZoomAndUIValues`          | `null` aka **nothing**                                         | Keybinds to change to zoom level and UI scale to the preset values.   |
  | `KeybindListMovementCameraUp`               | `null` aka **nothing**                                         | Keybinds to change the camera a bit up and locks it.                  |
  | `KeybindListMovementCameraDown`             | `null` aka **nothing**                                         | Keybinds to change the camera a bit down and locks it.                |
  | `KeybindListMovementCameraLeft`             | `null` aka **nothing**                                         | Keybinds to change the camera a bit to the left and locks it.         |
  | `KeybindListMovementCameraRight`            | `null` aka **nothing**                                         | Keybinds to change the camera a bit to the right and locks it.        |
  | `KeybindListMovementCameraReset`            | `null` aka **nothing**                                         | Keybinds to reset the camera movement and unlocks it.                 |
  | `KeybindListToggleUIVisibility`             | `null` aka **nothing**                                         | Keybinds to toggle the UI Visibility.                                 |
  | `KeybindListToggleHideUIWithCertainZoom`    | `null` aka **nothing**                                         | Keybinds to hides the UI at a certain Zoom Level.                     |
  | `KeybindListToggleAnyKeyToResetCamera`      | `null` aka **nothing**                                         | Keybinds to toggle the 'Any Button Resets Camera'.                    |
  | `KeybindListToggleAutoZoomToCurrentMapSize` | `null` aka **nothing**                                         | Keybinds to toggle the 'Auto Zoom to Map Size'.                       |
  | `TogglePresetOnLoadSaveFile`                | `null` aka **nothing**                                         | Keybinds to toggle the 'PresetOnLoadSaveFile'.                        |

- Zoom, UI and Camera values:

  | Setting Name                 | Default Value     | Description                                    |
  | :--------------------------- | :---------------- | :--------------------------------------------- |
  | `ZoomOrUILevelIncreaseValue` | 0.05 aka **5%**   | The amount of Zoom or UI Level increase.       |
  | `ZoomOrUILevelDecreaseValue` | -0.05 aka **-5%** | The amount of Zoom or UI Level decrease.       |
  | `ResetZoomOrUIValue`         | 1.00 aka **100%** | The value of the Zoom or UI level reset.       |
  | `MaxZoomOrUIValue`           | 2.00 aka **200%** | The value of the max Zoom in Level or Min UI.  |
  | `MinZoomOrUIValue`           | 0.35 aka **35%**  | The value of the max Zoom out Level or Max UI. |
  | `ZoomLevelThatHidesUI`       | 0.35 aka **35%**  | The value of the Zoom level that hides the UI. |
  | `CameraMovementSpeedValue`   | 15                | The speed that the camera moves.               |
  | `PresetZoomLevelValue`       | 0.86 aka **86%**  | The value of the preset Zoom level.            |
  | `PresetUIScaleValue`         | 0.75 aka **75%**  | The value of the preset UI Scale.              |

- Other options:

  | Setting Name                 | Default Value | Description                                                                                            |
  | :--------------------------- | :------------ | :----------------------------------------------------------------------------------------------------- |
  | `SuppressControllerButtons`  | `true`        | If your controller inputs are suppressed or not.                                                       |
  | `AutoZoomToCurrentMapSize`   | `false`       | If activated it auto zooms to map size.                                                                |
  | `AnyButtonToCenterCamera`    | `true`        | If activated any key you press will center the Camera.                                                 |
  | `HideUIWithCertainZoom`      | `false`       | If activated your UI hides when it reaches a certain zoom level.                                       |
  | `PresetOnLoadSaveFile`       | `false`       | If activated your UI hides when it reaches a certain zoom level.                                       |
  | `ZoomAndUIControlEverywhere` | `false`       | If activated your preset values will update when the file loads with the values set on the value tabs. |

## Compatibility

ZoomLevel is compatible with Stardew Valley 1.5+ on Linux/Mac/Windows, both single-player, local co-op and multiplayer.

## Changelog

- [Full Changelog](https://github.com/thespbgamer/ZoomLevel/blob/main/CHANGELOG.md#full-changelog)

## See also

- [Nexus mod](https://www.nexusmods.com/stardewvalley/mods/7363/?tab=files)
- [CurseForge mod](https://www.curseforge.com/stardewvalley/mods/zoom-level/files)
- [The mod that this mod was based by](https://github.com/GuiNoya/SVMods/)
