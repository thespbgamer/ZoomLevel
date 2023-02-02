using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            configsForTheMod = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += this.OnLaunched;
            helper.Events.Input.ButtonPressed += this.Events_Input_ButtonPressed;

            //On area change and on load save
            helper.Events.Player.Warped += this.Player_Warped;
            helper.Events.GameLoop.SaveLoaded += this.GameLoop_SaveLoaded;
        }

        private void OnLaunched(object sender, GameLaunchedEventArgs e)
        {
            var genericModConfigMenuAPI = Helper.ModRegistry.GetApi<IGenericModConfigMenuAPI>("spacechase0.GenericModConfigMenu");

            if (genericModConfigMenuAPI != null)

            {
                genericModConfigMenuAPI.Register(ModManifest, () => configsForTheMod = new ModConfig(), () => Helper.WriteConfig(configsForTheMod));

                genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("keybinds.title.name"), () => Helper.Translation.Get("keybinds.title.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListHoldToChangeUI, (KeybindList val) => configsForTheMod.KeybindListHoldToChangeUI = val, () => Helper.Translation.Get("keybinds.UIHoldKey.name"), () => Helper.Translation.Get("keybinds.UIHoldKey.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListIncreaseZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListIncreaseZoomOrUI = val, () => Helper.Translation.Get("keybinds.ZoomOrUIIncrease.name"), () => Helper.Translation.Get("keybinds.ZoomOrUIIncrease.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListDecreaseZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListDecreaseZoomOrUI = val, () => Helper.Translation.Get("keybinds.ZoomOrUIDecrease.name"), () => Helper.Translation.Get("keybinds.ZoomOrUIDecrease.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListResetZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListResetZoomOrUI = val, () => Helper.Translation.Get("keybinds.ZoomOrUIReset.name"), () => Helper.Translation.Get("keybinds.ZoomOrUIReset.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMaxZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListMaxZoomOrUI = val, () => Helper.Translation.Get("keybinds.ZoomOrUIMaxLevels.name"), () => Helper.Translation.Get("keybinds.ZoomOrUIMaxLevels.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMinZoomOrUI, (KeybindList val) => configsForTheMod.KeybindListMinZoomOrUI = val, () => Helper.Translation.Get("keybinds.ZoomOrUIMinLevels.name"), () => Helper.Translation.Get("keybinds.ZoomOrUIMinLevels.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListToggleUI, (KeybindList val) => configsForTheMod.KeybindListToggleUI = val, () => Helper.Translation.Get("keybinds.ToggleUIVisibility.name"), () => Helper.Translation.Get("keybinds.ToggleUIVisibility.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListToggleHideUIWithCertainZoom, (KeybindList val) => configsForTheMod.KeybindListToggleHideUIWithCertainZoom = val, () => Helper.Translation.Get("keybinds.ToggleHideUIAtCertainZoom.name"), () => Helper.Translation.Get("keybinds.ToggleHideUIAtCertainZoom.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListChangeZoomToApproximateCurrentMapSize, (KeybindList val) => configsForTheMod.KeybindListChangeZoomToApproximateCurrentMapSize = val, () => Helper.Translation.Get("keybinds.ChangeZoomToApproximateCurrentMapSize.name"), () => Helper.Translation.Get("keybinds.ChangeZoomToApproximateCurrentMapSize.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraLeft, (KeybindList val) => configsForTheMod.KeybindListMovementCameraLeft = val, () => Helper.Translation.Get("keybinds.CameraMovementLeft.name"), () => Helper.Translation.Get("keybinds.CameraMovementLeft.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraRight, (KeybindList val) => configsForTheMod.KeybindListMovementCameraRight = val, () => Helper.Translation.Get("keybinds.CameraMovementRight.name"), () => Helper.Translation.Get("keybinds.CameraMovementRight.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraUp, (KeybindList val) => configsForTheMod.KeybindListMovementCameraUp = val, () => Helper.Translation.Get("keybinds.CameraMovementUp.name"), () => Helper.Translation.Get("keybinds.CameraMovementUp.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListMovementCameraDown, (KeybindList val) => configsForTheMod.KeybindListMovementCameraDown = val, () => Helper.Translation.Get("keybinds.CameraMovementDown.name"), () => Helper.Translation.Get("keybinds.CameraMovementDown.description"));
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => configsForTheMod.KeybindListResetCameraMovement, (KeybindList val) => configsForTheMod.KeybindListResetCameraMovement = val, () => Helper.Translation.Get("keybinds.CameraMovementReset.name"), () => Helper.Translation.Get("keybinds.CameraMovementReset.description"));

                genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("values.title.name"), () => Helper.Translation.Get("values.title.description"));
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ZoomLevelIncreaseValue, (float val) => configsForTheMod.ZoomLevelIncreaseValue = val, () => Helper.Translation.Get("values.ZoomOrUILevelsIncrease.name"), () => Helper.Translation.Get("values.ZoomOrUILevelsIncrease.description"), 0.01f, 0.50f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ZoomLevelDecreaseValue, (float val) => configsForTheMod.ZoomLevelDecreaseValue = val, () => Helper.Translation.Get("values.ZoomOrUILevelsDecrease.name"), () => Helper.Translation.Get("values.ZoomOrUILevelsDecrease.description"), -0.50f, -0.01f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.MaxZoomInLevelAndUIValue, (float val) => configsForTheMod.MaxZoomInLevelAndUIValue = val, () => Helper.Translation.Get("values.ZoomOrUIMaxLevel.name"), () => Helper.Translation.Get("values.ZoomOrUIMaxLevel.description"), 1f, 2.5f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.MaxZoomOutLevelAndUIValue, (float val) => configsForTheMod.MaxZoomOutLevelAndUIValue = val, () => Helper.Translation.Get("values.ZoomOrUIMinLevel.name"), () => Helper.Translation.Get("values.ZoomOrUIMinLevel.description"), 0.15f, 1f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ResetZoomOrUIValue, (float val) => configsForTheMod.ResetZoomOrUIValue = val, () => Helper.Translation.Get("values.ZoomOrUIResetLevel.name"), () => Helper.Translation.Get("values.ZoomOrUIResetLevel.description"), 0.15f, 2.5f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.ZoomLevelThatHidesUI, (float val) => configsForTheMod.ZoomLevelThatHidesUI = val, () => Helper.Translation.Get("values.ZoomLevelThatHidesUI.name"), () => Helper.Translation.Get("values.ZoomLevelThatHidesUI.description"), 0.15f, 2.5f, 0.01f, FormatPercentage);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => configsForTheMod.CameraMovementSpeed, (int val) => configsForTheMod.CameraMovementSpeed = val, () => Helper.Translation.Get("values.CameraMovementSpeed.name"), () => Helper.Translation.Get("values.CameraMovementSpeed.description"), 1, 100, 1);

                genericModConfigMenuAPI.AddSectionTitle(ModManifest, () => Helper.Translation.Get("others.title.name"), () => Helper.Translation.Get("others.title.description"));
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.SuppressControllerButton, (bool val) => configsForTheMod.SuppressControllerButton = val, () => Helper.Translation.Get("others.SuppressControllerButtons.name"), () => Helper.Translation.Get("others.SuppressControllerButtons.description"));
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.ZoomAndUIControlEverywhere, (bool val) => configsForTheMod.ZoomAndUIControlEverywhere = val, () => Helper.Translation.Get("others.ZoomAndUIAnywhere.name"), () => Helper.Translation.Get("others.ZoomAndUIAnywhere.description"));
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.IsHideUIWithCertainZoom, (bool val) => configsForTheMod.IsHideUIWithCertainZoom = val, () => Helper.Translation.Get("others.HideUIWithCertainZoom.name"), () => Helper.Translation.Get("others.HideUIWithCertainZoom.description"));
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.PressAnyButtonToCenterCamera, (bool val) => configsForTheMod.PressAnyButtonToCenterCamera = val, () => Helper.Translation.Get("others.PressAnyButtonToCenterCamera.name"), () => Helper.Translation.Get("others.PressAnyButtonToCenterCamera.description"));
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => configsForTheMod.AutoZoomToMapSize, (bool val) => configsForTheMod.AutoZoomToMapSize = val, () => Helper.Translation.Get("others.AutoZoomToMapSize.name"), () => Helper.Translation.Get("others.AutoZoomToMapSize.description"));
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
                    ChangeUILevel(configsForTheMod.ZoomLevelIncreaseValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (configsForTheMod.KeybindListDecreaseZoomOrUI.JustPressed())
                {
                    ChangeUILevel(configsForTheMod.ZoomLevelDecreaseValue);
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
                    ToggleUIScale();
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (configsForTheMod.KeybindListToggleHideUIWithCertainZoom.JustPressed())
                {
                    ToggleHideWithUIWithCertainZoom();
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (configsForTheMod.KeybindListChangeZoomToApproximateCurrentMapSize.JustPressed())
                {
                    ChangeZoomLevelToCurrentMapSize();
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
            else if (configsForTheMod.KeybindListChangeZoomToApproximateCurrentMapSize.JustPressed())
            {
                ChangeZoomLevelToCurrentMapSize();
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (configsForTheMod.KeybindListMovementCameraUp.JustPressed())
            {
                if (Game1.viewport.Y > 0)
                {
                    wasCameraFrozen = true;
                    Game1.viewportFreeze = true;
                    Game1.viewport.Y -= configsForTheMod.CameraMovementSpeed;
                }
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (configsForTheMod.KeybindListMovementCameraDown.JustPressed())
            {
                if (Game1.viewport.Y < Game1.currentLocation.map.DisplayHeight - Game1.viewport.Height)
                {
                    wasCameraFrozen = true;
                    Game1.viewportFreeze = true;
                    Game1.viewport.Y += configsForTheMod.CameraMovementSpeed;
                }
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (configsForTheMod.KeybindListMovementCameraLeft.JustPressed())
            {
                if (Game1.viewport.X > 0)
                {
                    wasCameraFrozen = true;
                    Game1.viewportFreeze = true;
                    Game1.viewport.X -= configsForTheMod.CameraMovementSpeed;
                }
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (configsForTheMod.KeybindListMovementCameraRight.JustPressed())
            {
                if (Game1.viewport.X < Game1.currentLocation.map.DisplayWidth - Game1.viewport.Width)
                {
                    wasCameraFrozen = true;
                    Game1.viewportFreeze = true;
                    Game1.viewport.X += configsForTheMod.CameraMovementSpeed;
                }
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (wasCameraFrozen == true && Game1.viewportFreeze == true && (configsForTheMod.KeybindListResetCameraMovement.JustPressed() || configsForTheMod.PressAnyButtonToCenterCamera == true))
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

        private void ToggleHideWithUIWithCertainZoom()
        {
            configsForTheMod.IsHideUIWithCertainZoom = !configsForTheMod.IsHideUIWithCertainZoom;
            Game1.addHUDMessage(new HUDMessage(Helper.Translation.Get("hudMessages.HideUIWithCertainZoomIs.message", new { value = configsForTheMod.IsHideUIWithCertainZoom.ToString() }), 2));
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

        private void ToggleUIScale()
        {
            float uiValue = 0.0f;

            if ((configsForTheMod.IsHideUIWithCertainZoom == true && wasZoomLevelChanged == true))
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

        private void ChangeZoomLevel(float amount = 0)
        {
            float zoomLevelValue = (float)Math.Round(Game1.options.desiredBaseZoomLevel + amount, 2);

            UpdateZoomValue(zoomLevelValue);
        }

        private void ChangeUILevel(float amount = 0)
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
    }

    //Generic Mod Config Menu API
    namespace GenericModConfigMenu
    {
        /// <summary>The API which lets other mods add a config UI through Generic Mod Config Menu.</summary>
        public interface IGenericModConfigMenuAPI
        {
            /*********
            ** Methods
            *********/
            /****
            ** Must be called first
            ****/

            /// <summary>Register a mod whose config can be edited through the UI.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="reset">Reset the mod's config to its default values.</param>
            /// <param name="save">Save the mod's current config to the <c>config.json</c> file.</param>
            /// <param name="titleScreenOnly">Whether the options can only be edited from the title screen.</param>
            /// <remarks>Each mod can only be registered once, unless it's deleted via <see cref="Unregister"/> before calling this again.</remarks>
            void Register(IManifest mod, Action reset, Action save, bool titleScreenOnly = false);

            /****
            ** Basic options
            ****/

            /// <summary>Add a section title at the current position in the form.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="text">The title text shown in the form.</param>
            /// <param name="tooltip">The tooltip text shown when the cursor hovers on the title, or <c>null</c> to disable the tooltip.</param>
            void AddSectionTitle(IManifest mod, Func<string> text, Func<string> tooltip = null);

            /// <summary>Add a paragraph of text at the current position in the form.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="text">The paragraph text to display.</param>
            void AddParagraph(IManifest mod, Func<string> text);

            /// <summary>Add an image at the current position in the form.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="texture">The image texture to display.</param>
            /// <param name="texturePixelArea">The pixel area within the texture to display, or <c>null</c> to show the entire image.</param>
            /// <param name="scale">The zoom factor to apply to the image.</param>
            void AddImage(IManifest mod, Func<Texture2D> texture, Rectangle? texturePixelArea = null, int scale = Game1.pixelZoom);

            /// <summary>Add a boolean option at the current position in the form.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="getValue">Get the current value from the mod config.</param>
            /// <param name="setValue">Set a new value in the mod config.</param>
            /// <param name="name">The label text to show in the form.</param>
            /// <param name="tooltip">The tooltip text shown when the cursor hovers on the field, or <c>null</c> to disable the tooltip.</param>
            /// <param name="fieldId">The unique field ID for use with <see cref="OnFieldChanged"/>, or <c>null</c> to auto-generate a randomized ID.</param>
            void AddBoolOption(IManifest mod, Func<bool> getValue, Action<bool> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);

            /// <summary>Add an integer option at the current position in the form.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="getValue">Get the current value from the mod config.</param>
            /// <param name="setValue">Set a new value in the mod config.</param>
            /// <param name="name">The label text to show in the form.</param>
            /// <param name="tooltip">The tooltip text shown when the cursor hovers on the field, or <c>null</c> to disable the tooltip.</param>
            /// <param name="min">The minimum allowed value, or <c>null</c> to allow any.</param>
            /// <param name="max">The maximum allowed value, or <c>null</c> to allow any.</param>
            /// <param name="interval">The interval of values that can be selected.</param>
            /// <param name="formatValue">Get the display text to show for a value, or <c>null</c> to show the number as-is.</param>
            /// <param name="fieldId">The unique field ID for use with <see cref="OnFieldChanged"/>, or <c>null</c> to auto-generate a randomized ID.</param>
            void AddNumberOption(IManifest mod, Func<int> getValue, Action<int> setValue, Func<string> name, Func<string> tooltip = null, int? min = null, int? max = null, int? interval = null, Func<int, string> formatValue = null, string fieldId = null);

            /// <summary>Add a float option at the current position in the form.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="getValue">Get the current value from the mod config.</param>
            /// <param name="setValue">Set a new value in the mod config.</param>
            /// <param name="name">The label text to show in the form.</param>
            /// <param name="tooltip">The tooltip text shown when the cursor hovers on the field, or <c>null</c> to disable the tooltip.</param>
            /// <param name="min">The minimum allowed value, or <c>null</c> to allow any.</param>
            /// <param name="max">The maximum allowed value, or <c>null</c> to allow any.</param>
            /// <param name="interval">The interval of values that can be selected.</param>
            /// <param name="formatValue">Get the display text to show for a value, or <c>null</c> to show the number as-is.</param>
            /// <param name="fieldId">The unique field ID for use with <see cref="OnFieldChanged"/>, or <c>null</c> to auto-generate a randomized ID.</param>
            void AddNumberOption(IManifest mod, Func<float> getValue, Action<float> setValue, Func<string> name, Func<string> tooltip = null, float? min = null, float? max = null, float? interval = null, Func<float, string> formatValue = null, string fieldId = null);

            /// <summary>Add a string option at the current position in the form.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="getValue">Get the current value from the mod config.</param>
            /// <param name="setValue">Set a new value in the mod config.</param>
            /// <param name="name">The label text to show in the form.</param>
            /// <param name="tooltip">The tooltip text shown when the cursor hovers on the field, or <c>null</c> to disable the tooltip.</param>
            /// <param name="allowedValues">The values that can be selected, or <c>null</c> to allow any.</param>
            /// <param name="formatAllowedValue">Get the display text to show for a value from <paramref name="allowedValues"/>, or <c>null</c> to show the values as-is.</param>
            /// <param name="fieldId">The unique field ID for use with <see cref="OnFieldChanged"/>, or <c>null</c> to auto-generate a randomized ID.</param>
            void AddTextOption(IManifest mod, Func<string> getValue, Action<string> setValue, Func<string> name, Func<string> tooltip = null, string[] allowedValues = null, Func<string, string> formatAllowedValue = null, string fieldId = null);

            /// <summary>Add a key binding at the current position in the form.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="getValue">Get the current value from the mod config.</param>
            /// <param name="setValue">Set a new value in the mod config.</param>
            /// <param name="name">The label text to show in the form.</param>
            /// <param name="tooltip">The tooltip text shown when the cursor hovers on the field, or <c>null</c> to disable the tooltip.</param>
            /// <param name="fieldId">The unique field ID for use with <see cref="OnFieldChanged"/>, or <c>null</c> to auto-generate a randomized ID.</param>
            void AddKeybind(IManifest mod, Func<SButton> getValue, Action<SButton> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);

            /// <summary>Add a key binding list at the current position in the form.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="getValue">Get the current value from the mod config.</param>
            /// <param name="setValue">Set a new value in the mod config.</param>
            /// <param name="name">The label text to show in the form.</param>
            /// <param name="tooltip">The tooltip text shown when the cursor hovers on the field, or <c>null</c> to disable the tooltip.</param>
            /// <param name="fieldId">The unique field ID for use with <see cref="OnFieldChanged"/>, or <c>null</c> to auto-generate a randomized ID.</param>
            void AddKeybindList(IManifest mod, Func<KeybindList> getValue, Action<KeybindList> setValue, Func<string> name, Func<string> tooltip = null, string fieldId = null);

            /****
            ** Multi-page management
            ****/

            /// <summary>Start a new page in the mod's config UI, or switch to that page if it already exists. All options registered after this will be part of that page.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="pageId">The unique page ID.</param>
            /// <param name="pageTitle">The page title shown in its UI, or <c>null</c> to show the <paramref name="pageId"/> value.</param>
            /// <remarks>You must also call <see cref="AddPageLink"/> to make the page accessible. This is only needed to set up a multi-page config UI. If you don't call this method, all options will be part of the mod's main config UI instead.</remarks>
            void AddPage(IManifest mod, string pageId, Func<string> pageTitle = null);

            /// <summary>Add a link to a page added via <see cref="AddPage"/> at the current position in the form.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="pageId">The unique ID of the page to open when the link is clicked.</param>
            /// <param name="text">The link text shown in the form.</param>
            /// <param name="tooltip">The tooltip text shown when the cursor hovers on the link, or <c>null</c> to disable the tooltip.</param>
            void AddPageLink(IManifest mod, string pageId, Func<string> text, Func<string> tooltip = null);

            /****
            ** Advanced
            ****/

            /// <summary>Add an option at the current position in the form using custom rendering logic.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="name">The label text to show in the form.</param>
            /// <param name="draw">Draw the option in the config UI. This is called with the sprite batch being rendered and the pixel position at which to start drawing.</param>
            /// <param name="tooltip">The tooltip text shown when the cursor hovers on the field, or <c>null</c> to disable the tooltip.</param>
            /// <param name="beforeMenuOpened">A callback raised just before the menu containing this option is opened.</param>
            /// <param name="beforeSave">A callback raised before the form's current values are saved to the config (i.e. before the <c>save</c> callback passed to <see cref="Register"/>).</param>
            /// <param name="afterSave">A callback raised after the form's current values are saved to the config (i.e. after the <c>save</c> callback passed to <see cref="Register"/>).</param>
            /// <param name="beforeReset">A callback raised before the form is reset to its default values (i.e. before the <c>reset</c> callback passed to <see cref="Register"/>).</param>
            /// <param name="afterReset">A callback raised after the form is reset to its default values (i.e. after the <c>reset</c> callback passed to <see cref="Register"/>).</param>
            /// <param name="beforeMenuClosed">A callback raised just before the menu containing this option is closed.</param>
            /// <param name="height">The pixel height to allocate for the option in the form, or <c>null</c> for a standard input-sized option. This is called and cached each time the form is opened.</param>
            /// <param name="fieldId">The unique field ID for use with <see cref="OnFieldChanged"/>, or <c>null</c> to auto-generate a randomized ID.</param>
            /// <remarks>The custom logic represented by the callback parameters is responsible for managing its own state if needed. For example, you can store state in a static field or use closures to use a state variable.</remarks>
            void AddComplexOption(IManifest mod, Func<string> name, Action<SpriteBatch, Vector2> draw, Func<string> tooltip = null, Action beforeMenuOpened = null, Action beforeSave = null, Action afterSave = null, Action beforeReset = null, Action afterReset = null, Action beforeMenuClosed = null, Func<int> height = null, string fieldId = null);

            /// <summary>Set whether the options registered after this point can only be edited from the title screen.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="titleScreenOnly">Whether the options can only be edited from the title screen.</param>
            /// <remarks>This lets you have different values per-field. Most mods should just set it once in <see cref="Register"/>.</remarks>
            void SetTitleScreenOnlyForNextOptions(IManifest mod, bool titleScreenOnly);

            /// <summary>Register a method to notify when any option registered by this mod is edited through the config UI.</summary>
            /// <param name="mod">The mod's manifest.</param>
            /// <param name="onChange">The method to call with the option's unique field ID and new value.</param>
            /// <remarks>Options use a randomized ID by default; you'll likely want to specify the <c>fieldId</c> argument when adding options if you use this.</remarks>
            void OnFieldChanged(IManifest mod, Action<string, object> onChange);

            /// <summary>Open the config UI for a specific mod.</summary>
            /// <param name="mod">The mod's manifest.</param>
            void OpenModMenu(IManifest mod);

            /// <summary>Get the currently-displayed mod config menu, if any.</summary>
            /// <param name="mod">The manifest of the mod whose config menu is being shown, or <c>null</c> if not applicable.</param>
            /// <param name="page">The page ID being shown for the current config menu, or <c>null</c> if not applicable. This may be <c>null</c> even if a mod config menu is shown (e.g. because the mod doesn't have pages).</param>
            /// <returns>Returns whether a mod config menu is being shown.</returns>
            bool TryGetCurrentMenu(out IManifest mod, out string page);

            /// <summary>Remove a mod from the config UI and delete all its options and pages.</summary>
            /// <param name="mod">The mod's manifest.</param>
            void Unregister(IManifest mod);
        }
    }
}