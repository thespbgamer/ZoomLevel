## Description and Features

**ZoomLevel** is a [Stardew Valley](http://stardewvalley.net/) mod based on [this mod](https://github.com/GuiNoya/SVMods/) and it allows you to change and adjust the zoom level and UI levels of the game.

* You can increase zoom level with ``, (comma)`` and decrease it with ``. (period)``.<br>
 By holding ``Left Shift`` or ``Right Shift`` and using the controls above, you can change the UI Scale.

* If you use a controller, you can also adjust it by pressing the ``left stick`` to decrease the zoom and ``right stick`` to increases the zoom.<br>
By holding ``Left Trigger & Right Trigger`` and using the controls above, you can change the UI Scale.

## Contents
* [Installation](#Installation)
* [Configure](#Configure)
* [Compatibility](#Compatibility)
* [Changelog](#Changelog)
* [See also](#see-also)

## Installation
1. [Install the latest version of SMAPI](https://smapi.io/).
2. Download the mod from [nexus mods](https://www.nexusmods.com/stardewvalley/mods/7363?tab=files) or from [github](https://github.com/thespbgamer/ZoomLevel/releases/).
3. Unzip the mod folder into your Stardew Valley/Mods.
4. Run the game using SMAPI.

## Configure
### In-game settings
If you have the [generic mod config menu](https://www.nexusmods.com/stardewvalley/mods/5098?tab=files) installed, the configuration process becomes much simpler, you can click the cog button (⚙) on the title screen or the "mod options" button at the bottom of
the in-game menu to configure the mod.

### `config.json` file
The mod creates a `config.json` file in its mod folder the first time you run it. You can open the file in a text editor like notepad to configure the mod.

Here's what you can change:

* Player controls:

  Setting Name                                           | Default Value                                                   | Description
  :----------------------------------------------------- | :-------------------------------------------------------------- | :-----------------------------------------------
  `KeybindListHoldToChangeUI`                            | `LeftShift` or `RightShift` or `LeftTrigger and RightTrigger"`  | Key you need to hold to change the UI.
  `KeybindListIncreaseZoomOrUI`                          | `OemPeriod` aka `.` or `RightStick`                             | Key to Increase Zoom or UI Level.
  `KeybindListDecreaseZoomOrUI`                          | `OemComma` aka `,` or `LeftStick`                               | Key to Decrease Zoom or UI Level.
  `KeybindListResetZoomOrUI`                             | `null` aka **nothing**                                          | Key to Reset the Zoom or UI Level.
  `KeybindListMaxZoomOrUI`                               | `null` aka **nothing**                                          | Key to Max the Zoom out or Maximize the UI.
  `KeybindListMinZoomOrUI`                               | `null` aka **nothing**                                          | Key to Max the Zoom in or Minimize the UI.
  `KeybindListZoomToApproximateCurrentMapSize`           | `null` aka **nothing**                                          | Keybinds to change to zoom level to the approximate current map size.
  `KeybindListMovementCameraLeft`                        | `null` aka **nothing**                                          | Keybinds to change the camera a bit to the left and locks it.
  `KeybindListMovementCameraRight`                       | `null` aka **nothing**                                          | Keybinds to change the camera a bit to the right and locks it.
  `KeybindListMovementCameraUp`                          | `null` aka **nothing**                                          | Keybinds to change the camera a bit up and locks it.
  `KeybindListMovementCameraDown`                        | `null` aka **nothing**                                          | Keybinds to change the camera a bit down and locks it.
  `KeybindListMovementCameraReset`                       | `null` aka **nothing**                                          | Keybinds to reset the camera movement and unlocks it.
  `KeybindListToggleHideUIWithCertainZoom`               | `null` aka **nothing**                                          | Keybinds to hides the UI at a certain Zoom Level.
  `KeybindListToggleUI`                                  | `null` aka **nothing**                                          | Keybinds to toggle the UI Visibility.
  `KeybindListToggleAnyKeyToResetCamera`                 | `null` aka **nothing**                                          | Keybinds to toggle the 'Any Button Resets Camera'.
  `KeybindListToggleAutoZoomMap`                         | `null` aka **nothing**                                          | Keybinds to toggle the 'Auto Zoom to Map Size'.
  
  
* Zoom, UI and Camera values:

  Setting Name                   | Default Value              | Description
  :----------------------------- | :------------------------- | :--------------------------------------------
  `ZoomLevelIncreaseValue`       |  0.05 aka **5%**           | The amount of Zoom or UI Level increase.
  `ZoomLevelDecreaseValue`       | -0.05 aka **-5%**          | The amount of Zoom or UI Level decrease.
  `MaxZoomOutLevelAndUIValue`    |  0.35 aka **35%**          | The value of the max Zoom out Level or Max UI.
  `MaxZoomInLevelAndUIValue`     |  2.00 aka **200%**         | The value of the max Zoom in Level or Min UI.
  `ResetZoomOrUIValue`           |  1.00 aka **100%**         | The value of the Zoom or UI level reset.
  `ZoomLevelThatHidesUI`         |  0.35 aka **35%**          | The value of the Zoom level that hides the UI.
  `CameraMovementSpeed`          |  15                        | The speed that the camera moves.

* Other options:

  Setting Name                         | Default Value   | Description
  :----------------------------------- | :-------------- | :-------------------------------------------------
  `SuppressControllerButton`           | `true`          | If your controller inputs are suppressed or not.
  `AutoZoomToMapSize`                  | `false`         | If activated it auto zooms to map size.
  `AnyButtonToCenterCamera`            | `true`          | If activated any key you press will center the Camera.
  `HideUIWithCertainZoom`              | `false`         | If activated your UI hides when it reaches a certain zoom level.
  `ZoomAndUIControlEverywhere`         | `false`         | If activated you can control your Zoom and UI Level anywhere.

## Compatibility
ZoomLevel is compatible with Stardew Valley 1.5+ on Linux/Mac/Windows, both single-player, local co-op and multiplayer.

## Changelog
* [Full Changelog](https://github.com/thespbgamer/ZoomLevel/blob/main/CHANGELOG.md#full-changelog)

## See also
* [Nexus mod](https://www.nexusmods.com/stardewvalley/mods/7363/?tab=files)
* [CurseForge mod](https://www.curseforge.com/stardewvalley/mods/zoom-level/files)
* [The mod that this mod was based by](https://github.com/GuiNoya/SVMods/)
