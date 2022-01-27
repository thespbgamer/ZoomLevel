## Description and Features

**ZoomLevel** is a [Stardew Valley](http://stardewvalley.net/) mod based on [this mod](https://github.com/GuiNoya/SVMods/) and it allows you to change and adjust the zoom level and UI levels of the game.

* You can increase zoom level with ``, (comma)`` and decrease it with ``. (period)``.<br>
 By holding ``Left Shift`` or ``Right Shift`` and using the controls above, you can change the UI Scale.

* If you use a controller, you can also adjust it by pressing the ``left stick`` to decrease the zoom and ``right stick`` to increases the zoom.<br>
By holding ``Left Trigger & Right Trigger`` and using the controls above, you can change the UI Scale.

## Contents
* [Installation](#install)
* [Configure](#configure)
* [Compatibility](#compatibility)
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

  Setting Name                     | Default Value                                                   | Description
  :------------------------------- | :-------------------------------------------------------------- | :------------------
  `KeybindListIncreaseZoomOrUI`    | `OemPeriod` aka `.` or `RightStick`                             | Key to Increase Zoom or UI Level.
  `KeybindListDecreaseZoomOrUI`    | `OemComma` aka `,` or `LeftStick`                               | Key to Decrease Zoom or UI Level.
  `KeybindListHoldToChangeUI`      | `LeftShift` or `RightShift` or `LeftTrigger and RightTrigger"`  | Key you need to hold to change the UI.
  `KeybindListResetZoomOrUI`       | `null` aka **nothing**                                          | Key to Reset the Zoom or UI Level.
  `KeybindListMaxZoomOrUI`         | `null` aka **nothing**                                          | Key to Max the Zoom out or Maximize the UI.
  `KeybindListMinZoomOrUI`         | `null` aka **nothing**                                          | Key to Max the Zoom in or Minimize the UI.
  
* Zoom and UI values:

  Setting Name                   | Default Value | Description
  :----------------------------- | :------------ | :------------------
  `ZoomLevelIncreaseValue`       |  0.05         | The amount of Zoom or UI Level increase.
  `ZoomLevelDecreaseValue`       | -0.05         | The amount of Zoom or UI Level decrease.
  `MaxZoomOutLevelAndUIValue`    |  0.35         | The value of the max Zoom out Level or Max UI.
  `MaxZoomInLevelAndUIValue`     | -0.35         | The value of the max Zoom in Level or Min UI.
  `ResetZoomOrUIValue`           |  1.00         | The value of the Zoom or UI level reset.

* Other options:

  Setting Name                    | Default Value   | Description
  :------------------------------ | :-------------- | :------------------
  `SuppressControllerButton`      | `true`          | If your controller inputs are suppressed or not.
  `ZoomAndUIControlEverywhere`    | `false`         | If activated you can control your Zoom and UI Level anywhere.

## Compatibility
ZoomLevel is compatible with Stardew Valley 1.5.5+ on Linux/Mac/Windows, both single-player, local co-op and
multiplayer.

## See also
* [Nexus mod](http://www.nexusmods.com/stardewvalley/mods/7363/)
* [The mod that this mod was based by](https://github.com/GuiNoya/SVMods/)