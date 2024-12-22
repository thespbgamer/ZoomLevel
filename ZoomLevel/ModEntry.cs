using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace ZoomLevel
{
  public partial class ModEntry : Mod
  {
    private ModConfig configsForTheMod = new();
    public Random? globalRNG = null;

    private bool wasThePreviousButtonPressSucessfull;
    private bool wasToggleUIScaleClicked;
    private bool wasZoomLevelChanged;
    private bool wasCameraFrozen;

    private float uiScaleBeforeHiddingTheUI;
    private float currentUIScale;
    private float currentZoomLevel;

    public override void Entry(IModHelper helper)
    {
      CommonHelper.RemoveObsoleteFiles(this, "ZoomLevel.pdb");

      configsForTheMod = helper.ReadConfig<ModConfig>();

      helper.Events.GameLoop.GameLaunched += this.Events_GameLoop_GameLaunched;
      helper.Events.Input.ButtonPressed += this.Events_Input_ButtonPressed;
      helper.Events.Input.ButtonsChanged += this.Events_Input_ButtonChanged;

      //On area change and on load save
      helper.Events.Player.Warped += this.Events_Player_Warped;
      helper.Events.GameLoop.SaveLoaded += this.Events_GameLoop_SaveLoaded;

      helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.toggleAutoZoomMap.name"), Helper.Translation.Get("consoleCommands.toggleAutoZoomMap.description"), this.ConsoleFunctionsList);
      helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.togglePressAnyKeyToResetCamera.name"), Helper.Translation.Get("consoleCommands.togglePressAnyKeyToResetCamera.description"), this.ConsoleFunctionsList);
      helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.toggleHideWithUIWithCertainZoom.name"), Helper.Translation.Get("consoleCommands.toggleHideWithUIWithCertainZoom.description"), this.ConsoleFunctionsList);
      helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.togglePresetOnLoadSaveFile.name"), Helper.Translation.Get("consoleCommands.togglePresetOnLoadSaveFile.description"), this.ConsoleFunctionsList);
      helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.resetUIAndZoom.name"), Helper.Translation.Get("consoleCommands.resetUIAndZoom.description"), this.ConsoleFunctionsList);
      helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.resetUI.name"), Helper.Translation.Get("consoleCommands.resetUI.description"), this.ConsoleFunctionsList);
      helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.resetZoom.name"), Helper.Translation.Get("consoleCommands.resetZoom.description"), this.ConsoleFunctionsList);
    }

    private void Events_GameLoop_GameLaunched(object? sender, GameLaunchedEventArgs e)
    {
      MakeMenusInGenericModMenu();
    }

    private void MakeMenusInGenericModMenu()
    {
      var genericModConfigMenuAPI = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

      if (genericModConfigMenuAPI != null)

      {
        genericModConfigMenuAPI.Register(ModManifest, () => configsForTheMod = new ModConfig(), () => Helper.WriteConfig(configsForTheMod), false);

        genericModConfigMenuAPI.AddPageLink(ModManifest, Helper.Translation.Get("pages.keybinds.id"), () => Helper.Translation.Get("pages.keybinds.displayedName"), () => Helper.Translation.Get("pages.keybinds.tooltip"));
        genericModConfigMenuAPI.AddPageLink(ModManifest, Helper.Translation.Get("pages.values.id"), () => Helper.Translation.Get("pages.values.displayedName"), () => Helper.Translation.Get("pages.values.tooltip"));
        genericModConfigMenuAPI.AddPageLink(ModManifest, Helper.Translation.Get("pages.miscellaneous.id"), () => Helper.Translation.Get("pages.miscellaneous.displayedName"), () => Helper.Translation.Get("pages.miscellaneous.tooltip"));
        genericModConfigMenuAPI.AddPageLink(ModManifest, Helper.Translation.Get("pages.randomizer.id"), () => Helper.Translation.Get("pages.randomizer.displayedName"), () => Helper.Translation.Get("pages.randomizer.tooltip"));

        genericModConfigMenuAPI.AddPage(ModManifest, Helper.Translation.Get("pages.keybinds.id"), () => Helper.Translation.Get("pages.keybinds.pageTitle"));

        genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("keybinds.subtitle.main.displayedName"), () => Helper.Translation.Get("keybinds.subtitle.main.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListHoldToChangeUI, (KeybindList val) => configsForTheMod.KeybindListHoldToChangeUI = val, () => Helper.Translation.Get("keybinds.HoldToChangeUI.displayedName"), () => Helper.Translation.Get("keybinds.HoldToChangeUI.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListIncreaseZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListIncreaseZoomOrUI = val, () => Helper.Translation.Get("keybinds.IncreaseZoomOrUI.displayedName"), () => Helper.Translation.Get("keybinds.IncreaseZoomOrUI.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListDecreaseZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListDecreaseZoomOrUI = val, () => Helper.Translation.Get("keybinds.DecreaseZoomOrUI.displayedName"), () => Helper.Translation.Get("keybinds.DecreaseZoomOrUI.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListResetZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListResetZoomOrUI = val, () => Helper.Translation.Get("keybinds.ResetZoomOrUI.displayedName"), () => Helper.Translation.Get("keybinds.ResetZoomOrUI.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMaxZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListMaxZoomOrUI = val, () => Helper.Translation.Get("keybinds.MaxZoomOrUI.displayedName"), () => Helper.Translation.Get("keybinds.MaxZoomOrUI.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMinZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListMinZoomOrUI = val, () => Helper.Translation.Get("keybinds.MinZoomOrUI.displayedName"), () => Helper.Translation.Get("keybinds.MinZoomOrUI.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListZoomToCurrentMapSize, (KeybindList val) => configsForTheMod.KeybindListZoomToCurrentMapSize = val, () => Helper.Translation.Get("keybinds.ZoomToCurrentMapSize.displayedName"), () => Helper.Translation.Get("keybinds.ZoomToCurrentMapSize.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListPresetZoomAndUIValues, (KeybindList val) => configsForTheMod.KeybindListPresetZoomAndUIValues = val, () => Helper.Translation.Get("keybinds.PresetZoomAndUIValues.displayedName"), () => Helper.Translation.Get("keybinds.PresetZoomAndUIValues.tooltip"));

        genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("keybinds.subtitle.camera.displayedName"), () => Helper.Translation.Get("keybinds.subtitle.camera.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraUp, (KeybindList val) => configsForTheMod.KeybindListMovementCameraUp = val, () => Helper.Translation.Get("keybinds.MovementCameraUp.displayedName"), () => Helper.Translation.Get("keybinds.MovementCameraUp.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraDown, (KeybindList val) => configsForTheMod.KeybindListMovementCameraDown = val, () => Helper.Translation.Get("keybinds.MovementCameraDown.displayedName"), () => Helper.Translation.Get("keybinds.MovementCameraDown.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraLeft, (KeybindList val) => configsForTheMod.KeybindListMovementCameraLeft = val, () => Helper.Translation.Get("keybinds.MovementCameraLeft.displayedName"), () => Helper.Translation.Get("keybinds.MovementCameraLeft.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraRight, (KeybindList val) => configsForTheMod.KeybindListMovementCameraRight = val, () => Helper.Translation.Get("keybinds.MovementCameraRight.displayedName"), () => Helper.Translation.Get("keybinds.MovementCameraRight.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraReset, (KeybindList val) => configsForTheMod.KeybindListMovementCameraReset = val, () => Helper.Translation.Get("keybinds.MovementCameraReset.displayedName"), () => Helper.Translation.Get("keybinds.MovementCameraReset.tooltip"));

        genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("keybinds.subtitle.toggle.displayedName"), () => Helper.Translation.Get("keybinds.subtitle.toggle.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListToggleUIVisibility, (KeybindList val) => configsForTheMod.KeybindListToggleUIVisibility = val, () => Helper.Translation.Get("keybinds.ToggleUIVisibility.displayedName"), () => Helper.Translation.Get("keybinds.ToggleUIVisibility.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListToggleHideUIWithCertainZoom, (KeybindList val) => configsForTheMod.KeybindListToggleHideUIWithCertainZoom = val, () => Helper.Translation.Get("keybinds.ToggleHideUIWithCertainZoom.displayedName"), () => Helper.Translation.Get("keybinds.ToggleHideUIWithCertainZoom.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListToggleAnyKeyToResetCamera, (KeybindList val) => configsForTheMod.KeybindListToggleAnyKeyToResetCamera = val, () => Helper.Translation.Get("keybinds.ToggleAnyKeyToResetCamera.displayedName"), () => Helper.Translation.Get("keybinds.ToggleAnyKeyToResetCamera.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListToggleAutoZoomToCurrentMapSize, (KeybindList val) => configsForTheMod.KeybindListToggleAutoZoomToCurrentMapSize = val, () => Helper.Translation.Get("keybinds.ToggleAutoZoomToCurrentMapSize.displayedName"), () => Helper.Translation.Get("keybinds.ToggleAutoZoomToCurrentMapSize.tooltip"));
        genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListTogglePresetOnLoadSaveFile, (KeybindList val) => configsForTheMod.KeybindListTogglePresetOnLoadSaveFile = val, () => Helper.Translation.Get("keybinds.TogglePresetOnLoadSaveFile.displayedName"), () => Helper.Translation.Get("keybinds.TogglePresetOnLoadSaveFile.tooltip"));

        genericModConfigMenuAPI.AddPage(ModManifest, Helper.Translation.Get("pages.values.id"), () => Helper.Translation.Get("pages.values.pageTitle"));
        genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("values.subtitle.main.displayedName"), () => Helper.Translation.Get("values.subtitle.main.tooltip"));
        genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ZoomOrUILevelIncreaseValue, (float val) => configsForTheMod.ZoomOrUILevelIncreaseValue = val, () => Helper.Translation.Get("values.ZoomOrUILevelIncreaseValue.displayedName"), () => Helper.Translation.Get("values.ZoomOrUILevelIncreaseValue.tooltip"), 0.01f, 0.50f, 0.01f, FormatPercentage);
        genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ZoomOrUILevelDecreaseValue, (float val) => configsForTheMod.ZoomOrUILevelDecreaseValue = val, () => Helper.Translation.Get("values.ZoomOrUILevelDecreaseValue.displayedName"), () => Helper.Translation.Get("values.ZoomOrUILevelDecreaseValue.tooltip"), -0.50f, -0.01f, 0.01f, FormatPercentage);
        genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ResetZoomOrUIValue, (float val) => configsForTheMod.ResetZoomOrUIValue = val, () => Helper.Translation.Get("values.ResetZoomOrUIValue.displayedName"), () => Helper.Translation.Get("values.ResetZoomOrUIValue.tooltip"), 0.15f, 2.5f, 0.01f, FormatPercentage);
        genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.MaxZoomOrUIValue, (float val) => configsForTheMod.MaxZoomOrUIValue = val, () => Helper.Translation.Get("values.MaxZoomOrUIValue.displayedName"), () => Helper.Translation.Get("values.MaxZoomOrUIValue.tooltip"), 1f, 2.5f, 0.01f, FormatPercentage);
        genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.MinZoomOrUIValue, (float val) => configsForTheMod.MinZoomOrUIValue = val, () => Helper.Translation.Get("values.MinZoomOrUIValue.displayedName"), () => Helper.Translation.Get("values.MinZoomOrUIValue.tooltip"), 0.15f, 1f, 0.01f, FormatPercentage);
        genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ZoomLevelThatHidesUI, (float val) => configsForTheMod.ZoomLevelThatHidesUI = val, () => Helper.Translation.Get("values.ZoomLevelThatHidesUI.displayedName"), () => Helper.Translation.Get("values.ZoomLevelThatHidesUI.tooltip"), 0.15f, 2.5f, 0.01f, FormatPercentage);
        genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.CameraMovementSpeedValue, (int val) => configsForTheMod.CameraMovementSpeedValue = val, () => Helper.Translation.Get("values.CameraMovementSpeedValue.displayedName"), () => Helper.Translation.Get("values.CameraMovementSpeedValue.tooltip"), 5, 50, 1);
        genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.PresetZoomLevelValue, (float val) => configsForTheMod.PresetZoomLevelValue = val, () => Helper.Translation.Get("values.PresetZoomLevelValue.displayedName"), () => Helper.Translation.Get("values.PresetZoomLevelValue.tooltip"), 0.15f, 2.5f, 0.01f, FormatPercentage);
        genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.PresetUIScaleValue, (float val) => configsForTheMod.PresetUIScaleValue = val, () => Helper.Translation.Get("values.PresetUIScaleValue.displayedName"), () => Helper.Translation.Get("values.PresetUIScaleValue.tooltip"), 0.15f, 2.5f, 0.01f, FormatPercentage);

        genericModConfigMenuAPI.AddPage(ModManifest, Helper.Translation.Get("pages.miscellaneous.id"), () => Helper.Translation.Get("pages.miscellaneous.pageTitle"));

        genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("miscellaneous.subtitle.main.displayedName"), () => Helper.Translation.Get("miscellaneous.subtitle.main.tooltip"));
        genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.SuppressControllerButtons, (bool val) => configsForTheMod.SuppressControllerButtons = val, () => Helper.Translation.Get("miscellaneous.SuppressControllerButtons.displayedName"), () => Helper.Translation.Get("miscellaneous.SuppressControllerButtons.tooltip"));
        genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.AutoZoomToCurrentMapSize, (bool val) => configsForTheMod.AutoZoomToCurrentMapSize = val, () => Helper.Translation.Get("miscellaneous.AutoZoomToCurrentMapSize.displayedName"), () => Helper.Translation.Get("miscellaneous.AutoZoomToCurrentMapSize.tooltip"));
        genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.AnyButtonToCenterCamera, (bool val) => configsForTheMod.AnyButtonToCenterCamera = val, () => Helper.Translation.Get("miscellaneous.AnyButtonToCenterCamera.displayedName"), () => Helper.Translation.Get("miscellaneous.AnyButtonToCenterCamera.tooltip"));
        genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.HideUIWithCertainZoom, (bool val) => configsForTheMod.HideUIWithCertainZoom = val, () => Helper.Translation.Get("miscellaneous.HideUIWithCertainZoom.displayedName"), () => Helper.Translation.Get("miscellaneous.HideUIWithCertainZoom.tooltip"));
        genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.PresetOnLoadSaveFile, (bool val) => configsForTheMod.PresetOnLoadSaveFile = val, () => Helper.Translation.Get("miscellaneous.PresetOnLoadSaveFile.displayedName"), () => Helper.Translation.Get("miscellaneous.PresetOnLoadSaveFile.tooltip"));
        genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.ZoomAndUIControlEverywhere, (bool val) => configsForTheMod.ZoomAndUIControlEverywhere = val, () => Helper.Translation.Get("miscellaneous.ZoomAndUIControlEverywhere.displayedName"), () => Helper.Translation.Get("miscellaneous.ZoomAndUIControlEverywhere.tooltip"));

        genericModConfigMenuAPI.AddPage(ModManifest, Helper.Translation.Get("pages.randomizer.id"), () => Helper.Translation.Get("pages.randomizer.pageTitle"));

        genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("randomizer.subtitle.main.displayedName"), () => Helper.Translation.Get("randomizer.subtitle.main.tooltip"));
        genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.RandomizerEnableGlobal, (bool val) => configsForTheMod.RandomizerEnableGlobal = val, () => Helper.Translation.Get("randomizer.RandomizerEnableGlobal.displayedName"), () => Helper.Translation.Get("randomizer.RandomizerEnableGlobal.tooltip"));
        genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.RandomizerSeedValue, (int val) => configsForTheMod.RandomizerSeedValue = val, () => Helper.Translation.Get("randomizer.RandomizerSeedValue.displayedName"), () => Helper.Translation.Get("randomizer.RandomizerSeedValue.tooltip"));
        genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.RandomizerEnableUIChanges, (bool val) => configsForTheMod.RandomizerEnableUIChanges = val, () => Helper.Translation.Get("randomizer.RandomizerEnableUIChanges.displayedName"), () => Helper.Translation.Get("randomizer.RandomizerEnableUIChanges.tooltip"));
        genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.RandomizerEnableOnMapChange, (bool val) => configsForTheMod.RandomizerEnableOnMapChange = val, () => Helper.Translation.Get("randomizer.RandomizerEnableOnMapChange.displayedName"), () => Helper.Translation.Get("randomizer.RandomizerEnableOnMapChange.tooltip"));
        genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.RandomizerEnableOnMapLoad, (bool val) => configsForTheMod.RandomizerEnableOnMapLoad = val, () => Helper.Translation.Get("randomizer.RandomizerEnableOnMapLoad.displayedName"), () => Helper.Translation.Get("randomizer.RandomizerEnableOnMapLoad.tooltip"));
      }
    }

    private void Events_GameLoop_SaveLoaded(object? sender, SaveLoadedEventArgs e)
    {
      uiScaleBeforeHiddingTheUI = Game1.options.desiredUIScale;
      wasThePreviousButtonPressSucessfull = false;
      wasToggleUIScaleClicked = false;
      wasZoomLevelChanged = false;
      wasCameraFrozen = false;

      if (configsForTheMod.AutoZoomToCurrentMapSize == true)
      {
        ChangeZoomLevelToCurrentMapSize();
      }

      if (configsForTheMod.PresetOnLoadSaveFile == true)
      {
        PresetZoomAndUIValues();
      }

      CheckRandomizer();
    }

    private void Events_Player_Warped(object? sender, WarpedEventArgs e)
    {
      if (configsForTheMod.AutoZoomToCurrentMapSize == true)
      {
        ChangeZoomLevelToCurrentMapSize();
      }

      CheckRandomizer();
    }

    private void Events_Input_ButtonChanged(object? sender, ButtonsChangedEventArgs e)
    {
      if (configsForTheMod.KeybindListMovementCameraUp.IsDown() && !configsForTheMod.KeybindListMovementCameraDown.IsDown())
      {
        if (Game1.viewport.Y > 0)
        {
          wasCameraFrozen = true;
          Game1.viewportFreeze = true;
          Game1.viewport.Y -= configsForTheMod.CameraMovementSpeedValue;
        }
      }
      else if (configsForTheMod.KeybindListMovementCameraDown.IsDown() && !configsForTheMod.KeybindListMovementCameraUp.IsDown())
      {
        if (Game1.viewport.Y < Game1.currentLocation.map.DisplayHeight - Game1.viewport.Height)
        {
          wasCameraFrozen = true;
          Game1.viewportFreeze = true;
          Game1.viewport.Y += configsForTheMod.CameraMovementSpeedValue;
        }
      }
      if (configsForTheMod.KeybindListMovementCameraLeft.IsDown() && !configsForTheMod.KeybindListMovementCameraRight.IsDown())
      {
        if (Game1.viewport.X > 0)
        {
          wasCameraFrozen = true;
          Game1.viewportFreeze = true;
          Game1.viewport.X -= configsForTheMod.CameraMovementSpeedValue;
        }
      }
      else if (configsForTheMod.KeybindListMovementCameraRight.IsDown() && !configsForTheMod.KeybindListMovementCameraLeft.IsDown())
      {
        if (Game1.viewport.X < Game1.currentLocation.map.DisplayWidth - Game1.viewport.Width)
        {
          wasCameraFrozen = true;
          Game1.viewportFreeze = true;
          Game1.viewport.X += configsForTheMod.CameraMovementSpeedValue;
        }
      }
    }

    private void Events_Input_ButtonPressed(object? sender, ButtonPressedEventArgs e)
    {
      if (!Context.IsWorldReady || (!Context.IsPlayerFree && !configsForTheMod.ZoomAndUIControlEverywhere)) { return; }

      wasThePreviousButtonPressSucessfull = false;
      wasToggleUIScaleClicked = false;
      wasZoomLevelChanged = false;

      if (configsForTheMod.KeybindListHoldToChangeUI.IsDown())
      {
        HandleUI();
      }

      if (wasThePreviousButtonPressSucessfull == false)
      {
        if (configsForTheMod.KeybindListIncreaseZoomOrUI.JustPressed())
        {
          ChangeZoomLevel(configsForTheMod.ZoomOrUILevelIncreaseValue);
          wasThePreviousButtonPressSucessfull = true;
        }
        else if (configsForTheMod.KeybindListDecreaseZoomOrUI.JustPressed())
        {
          ChangeZoomLevel(configsForTheMod.ZoomOrUILevelDecreaseValue);
          wasThePreviousButtonPressSucessfull = true;
        }
        else if (configsForTheMod.KeybindListResetZoomOrUI.JustPressed())
        {
          UpdateZoomLevel(configsForTheMod.ResetZoomOrUIValue);
          wasThePreviousButtonPressSucessfull = true;
        }
        else if (configsForTheMod.KeybindListMaxZoomOrUI.JustPressed())
        {
          UpdateZoomLevel(configsForTheMod.MaxZoomOrUIValue);
          wasThePreviousButtonPressSucessfull = true;
        }
        else if (configsForTheMod.KeybindListMinZoomOrUI.JustPressed())
        {
          UpdateZoomLevel(configsForTheMod.MinZoomOrUIValue);
          wasThePreviousButtonPressSucessfull = true;
        }
        else if (configsForTheMod.KeybindListPresetZoomAndUIValues.JustPressed())
        {
          PresetZoomAndUIValues();
        }
        else if (configsForTheMod.KeybindListToggleUIVisibility.JustPressed())
        {
          wasToggleUIScaleClicked = true;
          ToggleUIScale();
          wasThePreviousButtonPressSucessfull = true;
        }
        else if (configsForTheMod.KeybindListToggleHideUIWithCertainZoom.JustPressed())
        {
          ToggleHideWithUIWithCertainZoom();
          wasThePreviousButtonPressSucessfull = true;
        }
        else if (configsForTheMod.KeybindListToggleAnyKeyToResetCamera.JustPressed())
        {
          TogglePressAnyKeyToResetCamera();
        }
        else if (configsForTheMod.KeybindListToggleAutoZoomToCurrentMapSize.JustPressed())
        {
          ToggleAutoZoomMap();
        }
        else if (configsForTheMod.KeybindListTogglePresetOnLoadSaveFile.JustPressed())
        {
          TogglePresetOnLoadSaveFile();
        }
        else if (configsForTheMod.KeybindListZoomToCurrentMapSize.JustPressed())
        {
          ChangeZoomLevelToCurrentMapSize();
          wasThePreviousButtonPressSucessfull = true;
        }
        else if (configsForTheMod.KeybindListMovementCameraUp.JustPressed() || configsForTheMod.KeybindListMovementCameraDown.JustPressed() || configsForTheMod.KeybindListMovementCameraLeft.JustPressed() || configsForTheMod.KeybindListMovementCameraRight.JustPressed())
        {
          //Do nothing
        }
        else if (wasCameraFrozen == true && Game1.viewportFreeze == true && (configsForTheMod.KeybindListMovementCameraReset.JustPressed() == true || (configsForTheMod.AnyButtonToCenterCamera == true)))
        {
          if (!configsForTheMod.KeybindListHoldToChangeUI.JustPressed() && !configsForTheMod.KeybindListHoldToChangeUI.IsDown())
          {
            wasCameraFrozen = false;
            Game1.viewportFreeze = false;
            wasThePreviousButtonPressSucessfull = true;
          }
        }
      }

      if (configsForTheMod.SuppressControllerButtons == true && wasThePreviousButtonPressSucessfull == true)
      {
        Helper.Input.Suppress(e.Button);
      }
    }

    private void HandleUI()
    {
      if (configsForTheMod.KeybindListIncreaseZoomOrUI.JustPressed())
      {
        ChangeUIScale(configsForTheMod.ZoomOrUILevelIncreaseValue);
        wasThePreviousButtonPressSucessfull = true;
      }
      else if (configsForTheMod.KeybindListDecreaseZoomOrUI.JustPressed())
      {
        ChangeUIScale(configsForTheMod.ZoomOrUILevelDecreaseValue);
        wasThePreviousButtonPressSucessfull = true;
      }
      else if (configsForTheMod.KeybindListResetZoomOrUI.JustPressed())
      {
        UpdateUIScale(configsForTheMod.ResetZoomOrUIValue);
        wasThePreviousButtonPressSucessfull = true;
      }
      else if (configsForTheMod.KeybindListMaxZoomOrUI.JustPressed())
      {
        UpdateUIScale(configsForTheMod.MaxZoomOrUIValue);
        wasThePreviousButtonPressSucessfull = true;
      }
      else if (configsForTheMod.KeybindListMinZoomOrUI.JustPressed())
      {
        UpdateUIScale(configsForTheMod.MinZoomOrUIValue);
        wasThePreviousButtonPressSucessfull = true;
      }
    }

    private void ToggleAutoZoomMap()
    {
      configsForTheMod.AutoZoomToCurrentMapSize = !configsForTheMod.AutoZoomToCurrentMapSize;
      Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("hudMessages.AutoZoomToCurrentMapSize.message", new { value = configsForTheMod.AutoZoomToCurrentMapSize.ToString() }), HUDMessage.newQuest_type));
    }

    private void TogglePressAnyKeyToResetCamera()
    {
      configsForTheMod.AnyButtonToCenterCamera = !configsForTheMod.AnyButtonToCenterCamera;
      Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("hudMessages.AnyButtonToCenterCamera.message", new { value = configsForTheMod.AnyButtonToCenterCamera.ToString() }), HUDMessage.newQuest_type));
    }

    private void ToggleHideWithUIWithCertainZoom()
    {
      configsForTheMod.HideUIWithCertainZoom = !configsForTheMod.HideUIWithCertainZoom;
      Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("hudMessages.HideUIWithCertainZoom.message", new { value = configsForTheMod.HideUIWithCertainZoom.ToString() }), HUDMessage.newQuest_type));
    }

    private void ToggleUIScale()
    {
      float uiValue = 0.0f;

      if ((configsForTheMod.HideUIWithCertainZoom == true && wasZoomLevelChanged == true))
      {
        if (configsForTheMod.ZoomLevelThatHidesUI >= Game1.options.desiredBaseZoomLevel && currentUIScale > 0.0f)
        {
          UpdateUIScale(uiValue);
        }
        else if (configsForTheMod.ZoomLevelThatHidesUI < Game1.options.desiredBaseZoomLevel && currentUIScale <= 0.0f)
        {
          uiValue = uiScaleBeforeHiddingTheUI;
          UpdateUIScale(uiValue);
        }
      }

      if ((wasZoomLevelChanged == false && wasToggleUIScaleClicked == true))
      {
        if (currentUIScale > 0.0f)
        {
          UpdateUIScale(uiValue);
        }
        else
        {
          uiValue = uiScaleBeforeHiddingTheUI;
          UpdateUIScale(uiValue);
        }
      }
    }

    private void TogglePresetOnLoadSaveFile()
    {
      configsForTheMod.PresetOnLoadSaveFile = !configsForTheMod.PresetOnLoadSaveFile;
      Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("hudMessages.PresetOnLoadSaveFile.message", new { value = configsForTheMod.PresetOnLoadSaveFile.ToString() }), HUDMessage.newQuest_type));
    }

    private void PresetZoomAndUIValues()
    {
      UpdateUIScale(configsForTheMod.PresetUIScaleValue);
      UpdateZoomLevel(configsForTheMod.PresetZoomLevelValue);
    }

    private void ChangeZoomLevelToCurrentMapSize()
    {
      if (Game1.currentLocation != null)
      {
        int mapWidth = Game1.currentLocation.map.DisplayWidth;
        int mapHeight = Game1.currentLocation.map.DisplayHeight;
        int screenWidth = Game1.graphics.GraphicsDevice.Viewport.Width;
        int screenHeight = Game1.graphics.GraphicsDevice.Viewport.Height;
        float zoomLevel;
        if (mapWidth > mapHeight)
        {
          zoomLevel = (float)screenWidth / (float)mapWidth;
        }
        else
        {
          zoomLevel = (float)screenHeight / (float)mapHeight;
        }

        UpdateZoomLevel(zoomLevel);
      }
    }

    private void ChangeZoomLevel(float amountToAdd = 0)
    {
      float zoomLevelValue = (float)Math.Round(Game1.options.desiredBaseZoomLevel + amountToAdd, 2);

      UpdateZoomLevel(zoomLevelValue);
    }

    private void ChangeUIScale(float amountToAdd = 0)
    {
      float uiScale = (float)Math.Round(Game1.options.desiredUIScale + amountToAdd, 2);

      UpdateUIScale(uiScale);
    }

    private void UpdateZoomLevel(float zoomLevelValue)
    {
      //Caps Max Zoom In Level
      zoomLevelValue = zoomLevelValue >= configsForTheMod.MaxZoomOrUIValue ? configsForTheMod.MaxZoomOrUIValue : zoomLevelValue;

      //Caps Max Zoom Out Level
      zoomLevelValue = zoomLevelValue <= configsForTheMod.MinZoomOrUIValue ? configsForTheMod.MinZoomOrUIValue : zoomLevelValue;

      //Changes ZoomLevel
      Game1.options.desiredBaseZoomLevel = RoundUp(zoomLevelValue, 2);

      currentZoomLevel = Game1.options.desiredBaseZoomLevel;
      wasZoomLevelChanged = true;
      ToggleUIScale();
    }

    private void UpdateUIScale(float uiScale)
    {
      if (uiScale != 0)
      {
        //Caps Max UI Scale
        uiScale = uiScale >= configsForTheMod.MaxZoomOrUIValue ? configsForTheMod.MaxZoomOrUIValue : uiScale;

        //Caps Min UI Scale
        uiScale = uiScale <= configsForTheMod.MinZoomOrUIValue ? configsForTheMod.MinZoomOrUIValue : uiScale;
      }
      else
      {
        uiScaleBeforeHiddingTheUI = Game1.options.desiredUIScale;
      }

      //Changes UI Scale
      Game1.options.desiredUIScale = uiScale;

      currentUIScale = Game1.options.desiredUIScale;
    }

    private static string FormatPercentage(float val)
    {
      return $"{val:0.#%}";
    }

    public static float RoundUp(float input, int places)
    {
      float multiplier = (float)Math.Pow(10, Convert.ToDouble(places));
      return (float)(Math.Ceiling(input * multiplier) / multiplier);
    }

    private void ConsoleFunctionsList(string command, string[] args)
    {
      if (command.Equals(Helper.Translation.Get("consoleCommands.toggleAutoZoomMap.name").ToString(), StringComparison.CurrentCultureIgnoreCase))
      {
        this.ToggleAutoZoomMap();
        this.Monitor.Log(Helper.Translation.Get("consoleMessages.AutoZoomToCurrentMapSize.message", new { value = configsForTheMod.AutoZoomToCurrentMapSize.ToString() }), LogLevel.Info);
      }
      else if (command.Equals(Helper.Translation.Get("consoleCommands.togglePressAnyKeyToResetCamera.name").ToString(), StringComparison.CurrentCultureIgnoreCase))
      {
        this.TogglePressAnyKeyToResetCamera();
        this.Monitor.Log(Helper.Translation.Get("consoleMessages.AnyButtonToCenterCamera.message", new { value = configsForTheMod.AnyButtonToCenterCamera.ToString() }), LogLevel.Info);
      }
      else if (command.Equals(Helper.Translation.Get("consoleCommands.toggleHideWithUIWithCertainZoom.name").ToString(), StringComparison.CurrentCultureIgnoreCase))
      {
        this.ToggleHideWithUIWithCertainZoom();
        this.Monitor.Log(Helper.Translation.Get("consoleMessages.HideUIWithCertainZoom.message", new { value = configsForTheMod.HideUIWithCertainZoom.ToString() }), LogLevel.Info);
      }
      else if (command.Equals(Helper.Translation.Get("consoleCommands.togglePresetOnLoadSaveFile.name").ToString(), StringComparison.CurrentCultureIgnoreCase))
      {
        this.TogglePresetOnLoadSaveFile();
        this.Monitor.Log(Helper.Translation.Get("consoleMessages.PresetOnLoadSaveFile.message", new { value = configsForTheMod.PresetOnLoadSaveFile.ToString() }), LogLevel.Info);
      }
      else if (command.Equals(Helper.Translation.Get("consoleCommands.resetUIAndZoom.name").ToString(), StringComparison.CurrentCultureIgnoreCase))
      {
        float uiScaleValue = 1f;
        float zoomLevelValue = 1f;
        if (args.Length > 0 && float.TryParse(args[0], out float uiCustomScale))
        {
          uiScaleValue = uiCustomScale;
        }

        if (args.Length > 1 && float.TryParse(args[1], out float zoomCustomLevel))
        {
          zoomLevelValue = zoomCustomLevel;
        }

        UpdateUIScale(uiScaleValue);
        UpdateZoomLevel(zoomLevelValue);
        this.Monitor.Log(Helper.Translation.Get("consoleMessages.resetUIAndZoom.message", new { ui = currentUIScale.ToString(), zoom = currentZoomLevel.ToString() }), LogLevel.Info);
      }
      else if (command.Equals(Helper.Translation.Get("consoleCommands.resetUI.name").ToString(), StringComparison.CurrentCultureIgnoreCase))
      {
        float uiScaleValue = 1f;

        if (args.Length > 0 && float.TryParse(args[0], out float uiCustomScale))
        {
          uiScaleValue = uiCustomScale;
        }

        UpdateUIScale(uiScaleValue);
        this.Monitor.Log(Helper.Translation.Get("consoleMessages.resetUI.message", new { value = currentUIScale.ToString() }), LogLevel.Info);
      }
      else if (command.Equals(Helper.Translation.Get("consoleCommands.resetZoom.name").ToString(), StringComparison.CurrentCultureIgnoreCase))
      {
        float zoomLevelValue = 1f;
        if (args.Length > 0 && float.TryParse(args[0], out float zoomCustomLevel))
        {
          zoomLevelValue = zoomCustomLevel;
        }

        UpdateZoomLevel(zoomLevelValue);
        this.Monitor.Log(Helper.Translation.Get("consoleMessages.resetZoom.message", new { value = currentZoomLevel.ToString() }), LogLevel.Info);
      }
    }

    private void CheckRandomizer()
    {
      if (configsForTheMod.RandomizerEnableGlobal && configsForTheMod.RandomizerEnableOnMapChange)
      {
        if (configsForTheMod.RandomizerSeedValue >= 0 && globalRNG == null)
        {
          globalRNG = new(configsForTheMod.RandomizerSeedValue);
        }
        else globalRNG ??= new();

        UpdateZoomLevel(GenerateNextRangedFloat(configsForTheMod.MinZoomOrUIValue, configsForTheMod.MaxZoomOrUIValue));
        if (configsForTheMod.RandomizerEnableUIChanges)
        {
          UpdateUIScale(GenerateNextRangedFloat(configsForTheMod.MinZoomOrUIValue, configsForTheMod.MaxZoomOrUIValue));
        }
      }
    }

    private float GenerateNextRangedFloat(float min, float max)
    {
      if (globalRNG == null)
      {
        Random random = new();
        return (float)random.NextDouble() * (max - min) + min;
      }
      return (float)globalRNG.NextDouble() * (max - min) + min;
    }
  }
}