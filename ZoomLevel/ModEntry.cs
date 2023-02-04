using System;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using ZoomLevel.GenericModConfigMenu;

namespace ZoomLevel
{
    public class ModEntry : Mod
    {
        private ModConfig configsForTheMod;

        private bool wasThePreviousButtonPressSucessfull;
        private bool wasToggleUIScaleClicked;
        private bool wasZoomLevelChanged;
        private bool wasCameraFrozen;

        private float uiScaleBeforeTheHidding;
        private float currentUIScale;
        private float currentZoomLevel;

        public override void Entry(IModHelper helper)
        {
            configsForTheMod = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += this.OnLaunched;
            helper.Events.Input.ButtonPressed += this.Events_Input_ButtonPressed;
            helper.Events.Input.ButtonsChanged += this.Events_Input_ButtonChanged;

            //On area change and on load save
            helper.Events.Player.Warped += this.Player_Warped;
            helper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;

            helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.zoomLevelList.name"), Helper.Translation.Get("consoleCommands.zoomLevelList.description"), this.ConsoleFunctionsList);
            helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.toggleAutoZoomMap.name"), Helper.Translation.Get("consoleCommands.toggleAutoZoomMap.description"), this.ConsoleFunctionsList);
            helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.togglePressAnyKeyToResetCamera.name"), Helper.Translation.Get("consoleCommands.togglePressAnyKeyToResetCamera.description"), this.ConsoleFunctionsList);
            helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.toggleHideWithUIWithCertainZoom.name"), Helper.Translation.Get("consoleCommands.toggleHideWithUIWithCertainZoom.description"), this.ConsoleFunctionsList);
            helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.resetUIAndZoom.name"), Helper.Translation.Get("consoleCommands.resetUIAndZoom.description"), this.ConsoleFunctionsList);
            helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.resetUI.name"), Helper.Translation.Get("consoleCommands.resetUI.description"), this.ConsoleFunctionsList);
            helper.ConsoleCommands.Add(Helper.Translation.Get("consoleCommands.resetZoom.name"), Helper.Translation.Get("consoleCommands.resetZoom.description"), this.ConsoleFunctionsList);
        }

        private void OnLaunched(object sender, GameLaunchedEventArgs e)
        {
            var genericModConfigMenuAPI = Helper.ModRegistry.GetApi<IGenericModConfigMenuAPI>("spacechase0.GenericModConfigMenu");

            if (genericModConfigMenuAPI != null)

            {
                genericModConfigMenuAPI.Register(ModManifest, () => configsForTheMod = new ModConfig(), () => Helper.WriteConfig(configsForTheMod), false);

                genericModConfigMenuAPI.AddPageLink(ModManifest, Helper.Translation.Get("pages.keybinds.id"), () => Helper.Translation.Get("pages.keybinds.displayedName"), () => Helper.Translation.Get("pages.keybinds.tooltip"));
                genericModConfigMenuAPI.AddPageLink(ModManifest, Helper.Translation.Get("pages.values.id"), () => Helper.Translation.Get("pages.values.displayedName"), () => Helper.Translation.Get("pages.values.tooltip"));
                genericModConfigMenuAPI.AddPageLink(ModManifest, Helper.Translation.Get("pages.miscellaneous.id"), () => Helper.Translation.Get("pages.miscellaneous.displayedName"), () => Helper.Translation.Get("pages.miscellaneous.tooltip"));

                genericModConfigMenuAPI.AddPage(ModManifest, Helper.Translation.Get("pages.keybinds.id"), () => Helper.Translation.Get("pages.keybinds.pageTitle"));

                genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("keybinds.subtitle.main.displayedName"), () => Helper.Translation.Get("keybinds.subtitle.main.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListHoldToChangeUI, (KeybindList val) => configsForTheMod.KeybindListHoldToChangeUI = val, () => Helper.Translation.Get("keybinds.UIHoldKey.displayedName"), () => Helper.Translation.Get("keybinds.UIHoldKey.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListIncreaseZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListIncreaseZoomOrUI = val, () => Helper.Translation.Get("keybinds.ZoomOrUIIncrease.displayedName"), () => Helper.Translation.Get("keybinds.ZoomOrUIIncrease.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListDecreaseZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListDecreaseZoomOrUI = val, () => Helper.Translation.Get("keybinds.ZoomOrUIDecrease.displayedName"), () => Helper.Translation.Get("keybinds.ZoomOrUIDecrease.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListResetZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListResetZoomOrUI = val, () => Helper.Translation.Get("keybinds.ZoomOrUIReset.displayedName"), () => Helper.Translation.Get("keybinds.ZoomOrUIReset.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMaxZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListMaxZoomOrUI = val, () => Helper.Translation.Get("keybinds.ZoomOrUIMaxLevels.displayedName"), () => Helper.Translation.Get("keybinds.ZoomOrUIMaxLevels.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMinZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListMinZoomOrUI = val, () => Helper.Translation.Get("keybinds.ZoomOrUIMinLevels.displayedName"), () => Helper.Translation.Get("keybinds.ZoomOrUIMinLevels.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListZoomToApproximateCurrentMapSize, (KeybindList val) => configsForTheMod.KeybindListZoomToApproximateCurrentMapSize = val, () => Helper.Translation.Get("keybinds.ZoomToApproximateCurrentMapSize.displayedName"), () => Helper.Translation.Get("keybinds.ZoomToApproximateCurrentMapSize.tooltip"));

                genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("keybinds.subtitle.camera.displayedName"), () => Helper.Translation.Get("keybinds.subtitle.camera.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraUp, (KeybindList val) => configsForTheMod.KeybindListMovementCameraUp = val, () => Helper.Translation.Get("keybinds.CameraMovementUp.displayedName"), () => Helper.Translation.Get("keybinds.CameraMovementUp.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraDown, (KeybindList val) => configsForTheMod.KeybindListMovementCameraDown = val, () => Helper.Translation.Get("keybinds.CameraMovementDown.displayedName"), () => Helper.Translation.Get("keybinds.CameraMovementDown.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraLeft, (KeybindList val) => configsForTheMod.KeybindListMovementCameraLeft = val, () => Helper.Translation.Get("keybinds.CameraMovementLeft.displayedName"), () => Helper.Translation.Get("keybinds.CameraMovementLeft.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraRight, (KeybindList val) => configsForTheMod.KeybindListMovementCameraRight = val, () => Helper.Translation.Get("keybinds.CameraMovementRight.displayedName"), () => Helper.Translation.Get("keybinds.CameraMovementRight.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraReset, (KeybindList val) => configsForTheMod.KeybindListMovementCameraReset = val, () => Helper.Translation.Get("keybinds.CameraMovementReset.displayedName"), () => Helper.Translation.Get("keybinds.CameraMovementReset.tooltip"));

                genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("keybinds.subtitle.toggle.displayedName"), () => Helper.Translation.Get("keybinds.subtitle.toggle.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListToggleUI, (KeybindList val) => configsForTheMod.KeybindListToggleUI = val, () => Helper.Translation.Get("keybinds.ToggleUIVisibility.displayedName"), () => Helper.Translation.Get("keybinds.ToggleUIVisibility.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListToggleHideUIWithCertainZoom, (KeybindList val) => configsForTheMod.KeybindListToggleHideUIWithCertainZoom = val, () => Helper.Translation.Get("keybinds.ToggleHideUIAtCertainZoom.displayedName"), () => Helper.Translation.Get("keybinds.ToggleHideUIAtCertainZoom.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListToggleAnyKeyToResetCamera, (KeybindList val) => configsForTheMod.KeybindListToggleAnyKeyToResetCamera = val, () => Helper.Translation.Get("keybinds.ToggleAnyKeyToResetCamera.displayedName"), () => Helper.Translation.Get("keybinds.ToggleAnyKeyToResetCamera.tooltip"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListToggleAutoZoomMap, (KeybindList val) => configsForTheMod.KeybindListToggleAutoZoomMap = val, () => Helper.Translation.Get("keybinds.ToggleAutoZoomMap.displayedName"), () => Helper.Translation.Get("keybinds.ToggleAutoZoomMap.tooltip"));

                genericModConfigMenuAPI.AddPage(ModManifest, Helper.Translation.Get("pages.values.id"), () => Helper.Translation.Get("pages.values.pageTitle"));
                genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("values.subtitle.main.displayedName"), () => Helper.Translation.Get("values.subtitle.main.tooltip"));
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ZoomLevelIncreaseValue, (float val) => configsForTheMod.ZoomLevelIncreaseValue = val, () => Helper.Translation.Get("values.ZoomOrUILevelsIncrease.displayedName"), () => Helper.Translation.Get("values.ZoomOrUILevelsIncrease.tooltip"), 0.01f, 0.50f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ZoomLevelDecreaseValue, (float val) => configsForTheMod.ZoomLevelDecreaseValue = val, () => Helper.Translation.Get("values.ZoomOrUILevelsDecrease.displayedName"), () => Helper.Translation.Get("values.ZoomOrUILevelsDecrease.tooltip"), -0.50f, -0.01f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.MaxZoomInLevelAndUIValue, (float val) => configsForTheMod.MaxZoomInLevelAndUIValue = val, () => Helper.Translation.Get("values.ZoomOrUIMaxLevel.displayedName"), () => Helper.Translation.Get("values.ZoomOrUIMaxLevel.tooltip"), 1f, 2.5f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.MaxZoomOutLevelAndUIValue, (float val) => configsForTheMod.MaxZoomOutLevelAndUIValue = val, () => Helper.Translation.Get("values.ZoomOrUIMinLevel.displayedName"), () => Helper.Translation.Get("values.ZoomOrUIMinLevel.tooltip"), 0.15f, 1f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ResetZoomOrUIValue, (float val) => configsForTheMod.ResetZoomOrUIValue = val, () => Helper.Translation.Get("values.ZoomOrUIResetLevel.displayedName"), () => Helper.Translation.Get("values.ZoomOrUIResetLevel.tooltip"), 0.15f, 2.5f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ZoomLevelThatHidesUI, (float val) => configsForTheMod.ZoomLevelThatHidesUI = val, () => Helper.Translation.Get("values.ZoomLevelThatHidesUI.displayedName"), () => Helper.Translation.Get("values.ZoomLevelThatHidesUI.tooltip"), 0.15f, 2.5f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.CameraMovementSpeed, (int val) => configsForTheMod.CameraMovementSpeed = val, () => Helper.Translation.Get("values.CameraMovementSpeed.displayedName"), () => Helper.Translation.Get("values.CameraMovementSpeed.tooltip"), 5, 50, 1);

                genericModConfigMenuAPI.AddPage(ModManifest, Helper.Translation.Get("pages.miscellaneous.id"), () => Helper.Translation.Get("pages.miscellaneous.pageTitle"));

                genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("miscellaneous.subtitle.main.displayedName"), () => Helper.Translation.Get("miscellaneous.subtitle.main.tooltip"));
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.SuppressControllerButton, (bool val) => configsForTheMod.SuppressControllerButton = val, () => Helper.Translation.Get("miscellaneous.SuppressControllerButtons.displayedName"), () => Helper.Translation.Get("miscellaneous.SuppressControllerButtons.tooltip"));
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.AnyButtonToCenterCamera, (bool val) => configsForTheMod.AnyButtonToCenterCamera = val, () => Helper.Translation.Get("miscellaneous.PressAnyButtonToCenterCamera.displayedName"), () => Helper.Translation.Get("miscellaneous.PressAnyButtonToCenterCamera.tooltip"));
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.AutoZoomToMapSize, (bool val) => configsForTheMod.AutoZoomToMapSize = val, () => Helper.Translation.Get("miscellaneous.AutoZoomToMapSize.displayedName"), () => Helper.Translation.Get("miscellaneous.AutoZoomToMapSize.tooltip"));
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.HideUIWithCertainZoom, (bool val) => configsForTheMod.HideUIWithCertainZoom = val, () => Helper.Translation.Get("miscellaneous.HideUIWithCertainZoom.displayedName"), () => Helper.Translation.Get("miscellaneous.HideUIWithCertainZoom.tooltip"));
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.ZoomAndUIControlEverywhere, (bool val) => configsForTheMod.ZoomAndUIControlEverywhere = val, () => Helper.Translation.Get("miscellaneous.ZoomAndUIAnywhere.displayedName"), () => Helper.Translation.Get("miscellaneous.ZoomAndUIAnywhere.tooltip"));
            }
        }

        private void GameLoop_SaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            uiScaleBeforeTheHidding = Game1.options.desiredUIScale;
            wasThePreviousButtonPressSucessfull = false;
            wasToggleUIScaleClicked = false;
            wasZoomLevelChanged = false;
            wasCameraFrozen = false;

            if (configsForTheMod.AutoZoomToMapSize == true)
            {
                ChangeZoomLevelToCurrentMapSize();
            }
        }

        private void Player_Warped(object sender, WarpedEventArgs e)
        {
            if (configsForTheMod.AutoZoomToMapSize == true)
            {
                ChangeZoomLevelToCurrentMapSize();
            }
        }

        private void Events_Input_ButtonChanged(object sender, ButtonsChangedEventArgs e)
        {
            if (configsForTheMod.KeybindListMovementCameraUp.IsDown() && !configsForTheMod.KeybindListMovementCameraDown.IsDown())
            {
                if (Game1.viewport.Y > 0)
                {
                    wasCameraFrozen = true;
                    Game1.viewportFreeze = true;
                    Game1.viewport.Y -= configsForTheMod.CameraMovementSpeed;
                }
            }
            else if (configsForTheMod.KeybindListMovementCameraDown.IsDown() && !configsForTheMod.KeybindListMovementCameraUp.IsDown())
            {
                if (Game1.viewport.Y < Game1.currentLocation.map.DisplayHeight - Game1.viewport.Height)
                {
                    wasCameraFrozen = true;
                    Game1.viewportFreeze = true;
                    Game1.viewport.Y += configsForTheMod.CameraMovementSpeed;
                }
            }
            if (configsForTheMod.KeybindListMovementCameraLeft.IsDown() && !configsForTheMod.KeybindListMovementCameraRight.IsDown())
            {
                if (Game1.viewport.X > 0)
                {
                    wasCameraFrozen = true;
                    Game1.viewportFreeze = true;
                    Game1.viewport.X -= configsForTheMod.CameraMovementSpeed;
                }
            }
            else if (configsForTheMod.KeybindListMovementCameraRight.IsDown() && !configsForTheMod.KeybindListMovementCameraLeft.IsDown())
            {
                if (Game1.viewport.X < Game1.currentLocation.map.DisplayWidth - Game1.viewport.Width)
                {
                    wasCameraFrozen = true;
                    Game1.viewportFreeze = true;
                    Game1.viewport.X += configsForTheMod.CameraMovementSpeed;
                }
            }
        }

        private void Events_Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady || (!Context.IsPlayerFree && !configsForTheMod.ZoomAndUIControlEverywhere)) { return; }

            wasThePreviousButtonPressSucessfull = false;
            wasToggleUIScaleClicked = false;
            wasZoomLevelChanged = false;

            if (configsForTheMod.KeybindListHoldToChangeUI.IsDown())
            {
                if (configsForTheMod.KeybindListIncreaseZoomOrUI.JustPressed())
                {
                    ChangeUIScale(configsForTheMod.ZoomLevelIncreaseValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (configsForTheMod.KeybindListDecreaseZoomOrUI.JustPressed())
                {
                    ChangeUIScale(configsForTheMod.ZoomLevelDecreaseValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (configsForTheMod.KeybindListResetZoomOrUI.JustPressed())
                {
                    UpdateUIScale(configsForTheMod.ResetZoomOrUIValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (configsForTheMod.KeybindListMaxZoomOrUI.JustPressed())
                {
                    UpdateUIScale(configsForTheMod.MaxZoomInLevelAndUIValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (configsForTheMod.KeybindListMinZoomOrUI.JustPressed())
                {
                    UpdateUIScale(configsForTheMod.MaxZoomOutLevelAndUIValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (configsForTheMod.KeybindListToggleUI.JustPressed())
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
                else if (configsForTheMod.KeybindListZoomToApproximateCurrentMapSize.JustPressed())
                {
                    ChangeZoomLevelToCurrentMapSize();
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (configsForTheMod.KeybindListMovementCameraUp.JustPressed() || configsForTheMod.KeybindListMovementCameraDown.JustPressed() || configsForTheMod.KeybindListMovementCameraLeft.JustPressed() || configsForTheMod.KeybindListMovementCameraRight.JustPressed())
                {
                    //Do nothing
                }
                else if (wasCameraFrozen == true && Game1.viewportFreeze == true && (configsForTheMod.KeybindListMovementCameraReset.JustPressed() == true || configsForTheMod.AnyButtonToCenterCamera == true))
                {
                    wasCameraFrozen = false;
                    Game1.viewportFreeze = false;
                    wasThePreviousButtonPressSucessfull = true;
                }
            }
            else if (configsForTheMod.KeybindListIncreaseZoomOrUI.JustPressed())
            {
                ChangeZoomLevel(configsForTheMod.ZoomLevelIncreaseValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (configsForTheMod.KeybindListDecreaseZoomOrUI.JustPressed())
            {
                ChangeZoomLevel(configsForTheMod.ZoomLevelDecreaseValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (configsForTheMod.KeybindListResetZoomOrUI.JustPressed())
            {
                UpdateZoomValue(configsForTheMod.ResetZoomOrUIValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (configsForTheMod.KeybindListMaxZoomOrUI.JustPressed())
            {
                UpdateZoomValue(configsForTheMod.MaxZoomInLevelAndUIValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (configsForTheMod.KeybindListMinZoomOrUI.JustPressed())
            {
                UpdateZoomValue(configsForTheMod.MaxZoomOutLevelAndUIValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (configsForTheMod.KeybindListToggleUI.JustPressed())
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
            else if (configsForTheMod.KeybindListToggleAutoZoomMap.JustPressed())
            {
                ToggleAutoZoomMap();
            }
            else if (configsForTheMod.KeybindListZoomToApproximateCurrentMapSize.JustPressed())
            {
                ChangeZoomLevelToCurrentMapSize();
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (configsForTheMod.KeybindListMovementCameraUp.JustPressed() || configsForTheMod.KeybindListMovementCameraDown.JustPressed() || configsForTheMod.KeybindListMovementCameraLeft.JustPressed() || configsForTheMod.KeybindListMovementCameraRight.JustPressed())
            {
                //Do nothing
            }
            else if (wasCameraFrozen == true && Game1.viewportFreeze == true && (configsForTheMod.KeybindListMovementCameraReset.JustPressed() == true || configsForTheMod.AnyButtonToCenterCamera == true))
            {
                wasCameraFrozen = false;
                Game1.viewportFreeze = false;
                wasThePreviousButtonPressSucessfull = true;
            }

            if (configsForTheMod.SuppressControllerButton == true && wasThePreviousButtonPressSucessfull == true)
            {
                Helper.Input.Suppress(e.Button);
            }
        }

        private void ToggleAutoZoomMap()
        {
            configsForTheMod.AutoZoomToMapSize = !configsForTheMod.AutoZoomToMapSize;
            Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("hudMessages.AutoZoomToMapSize.message", new { value = configsForTheMod.AutoZoomToMapSize.ToString() }), HUDMessage.newQuest_type));
        }

        private void TogglePressAnyKeyToResetCamera()
        {
            configsForTheMod.AnyButtonToCenterCamera = !configsForTheMod.AnyButtonToCenterCamera;
            Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("hudMessages.PressAnyKeyToCenterCamera.message", new { value = configsForTheMod.AnyButtonToCenterCamera.ToString() }), HUDMessage.newQuest_type));
        }

        private void ToggleHideWithUIWithCertainZoom()
        {
            configsForTheMod.HideUIWithCertainZoom = !configsForTheMod.HideUIWithCertainZoom;
            Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("hudMessages.HideUIWithCertainZoomIs.message", new { value = configsForTheMod.HideUIWithCertainZoom.ToString() }), HUDMessage.newQuest_type));
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
                    uiValue = uiScaleBeforeTheHidding;
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
                    uiValue = uiScaleBeforeTheHidding;
                    UpdateUIScale(uiValue);
                }
            }
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
                UpdateZoomValue(zoomLevel);
            }
        }

        private void ChangeZoomLevel(float amount = 0)
        {
            float zoomLevelValue = (float)Math.Round(Game1.options.desiredBaseZoomLevel + amount, 2);

            UpdateZoomValue(zoomLevelValue);
        }

        private void ChangeUIScale(float amount = 0)
        {
            float uiScale = (float)Math.Round(Game1.options.desiredUIScale + amount, 2);

            UpdateUIScale(uiScale);
        }

        private void UpdateZoomValue(float zoomLevelValue)
        {
            //Caps Max Zoom In Level
            zoomLevelValue = zoomLevelValue >= configsForTheMod.MaxZoomInLevelAndUIValue ? configsForTheMod.MaxZoomInLevelAndUIValue : zoomLevelValue;

            //Caps Max Zoom Out Level
            zoomLevelValue = zoomLevelValue <= configsForTheMod.MaxZoomOutLevelAndUIValue ? configsForTheMod.MaxZoomOutLevelAndUIValue : zoomLevelValue;

            //Changes ZoomLevel
            Game1.options.desiredBaseZoomLevel = zoomLevelValue;

            currentZoomLevel = Game1.options.desiredBaseZoomLevel;
            wasZoomLevelChanged = true;
            ToggleUIScale();
        }

        private void UpdateUIScale(float uiScale)
        {
            if (uiScale != 0)
            {
                //Caps Max UI Scale
                uiScale = uiScale >= configsForTheMod.MaxZoomInLevelAndUIValue ? configsForTheMod.MaxZoomInLevelAndUIValue : uiScale;

                //Caps Min UI Scale
                uiScale = uiScale <= configsForTheMod.MaxZoomOutLevelAndUIValue ? configsForTheMod.MaxZoomOutLevelAndUIValue : uiScale;
            }
            else
            {
                uiScaleBeforeTheHidding = Game1.options.desiredUIScale;
            }

            //Changes UI Scale
            Game1.options.desiredUIScale = uiScale;

            currentUIScale = Game1.options.desiredUIScale;
        }

        private string FormatPercentage(float val)
        {
            return $"{val:0.#%}";
        }

        private void ConsoleFunctionsList(string command, string[] args)
        {
            if (command.ToLower() == Helper.Translation.Get("consoleCommands.zoomLevelList.name"))
            {
                this.Monitor.Log(Helper.Translation.Get("consoleMessages.listingOfCommands.message"), LogLevel.Info);
            }
            else if (command.ToLower() == Helper.Translation.Get("consoleCommands.toggleAutoZoomMap.name"))
            {
                this.ToggleAutoZoomMap();
                this.Monitor.Log(Helper.Translation.Get("consoleMessages.AutoZoomToMapSize.message", new { value = configsForTheMod.AutoZoomToMapSize.ToString() }), LogLevel.Info);
            }
            else if (command.ToLower() == Helper.Translation.Get("consoleCommands.togglePressAnyKeyToResetCamera.name"))
            {
                this.TogglePressAnyKeyToResetCamera();
                this.Monitor.Log(Helper.Translation.Get("consoleMessages.PressAnyKeyToCenterCamera.message", new { value = configsForTheMod.AnyButtonToCenterCamera.ToString() }), LogLevel.Info);
            }
            else if (command.ToLower() == Helper.Translation.Get("consoleCommands.toggleHideWithUIWithCertainZoom.name"))
            {
                this.ToggleHideWithUIWithCertainZoom();
                this.Monitor.Log(Helper.Translation.Get("consoleMessages.HideUIWithCertainZoomIs.message", new { value = configsForTheMod.HideUIWithCertainZoom.ToString() }), LogLevel.Info);
            }
            else if (command.ToLower() == Helper.Translation.Get("consoleCommands.resetUIAndZoom.name"))
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
                UpdateZoomValue(zoomLevelValue);
                this.Monitor.Log(Helper.Translation.Get("consoleMessages.resetUIAndZoom.message", new { ui = currentUIScale.ToString(), zoom = currentZoomLevel.ToString() }), LogLevel.Info);
            }
            else if (command.ToLower() == Helper.Translation.Get("consoleCommands.resetUI.name"))
            {
                float uiScaleValue = 1f;

                if (args.Length > 0 && float.TryParse(args[0], out float uiCustomScale))
                {
                    uiScaleValue = uiCustomScale;
                }

                UpdateUIScale(uiScaleValue);
                this.Monitor.Log(Helper.Translation.Get("consoleMessages.resetUI.message", new { value = currentUIScale.ToString() }), LogLevel.Info);
            }
            else if (command.ToLower() == Helper.Translation.Get("consoleCommands.resetZoom.name"))
            {
                float zoomLevelValue = 1f;
                if (args.Length > 0 && float.TryParse(args[0], out float zoomCustomLevel))
                {
                    zoomLevelValue = zoomCustomLevel;
                }

                UpdateZoomValue(zoomLevelValue);
                this.Monitor.Log(Helper.Translation.Get("consoleMessages.resetZoom.message", new { value = currentZoomLevel.ToString() }), LogLevel.Info);
            }
        }
    }
}