using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace ZoomLevel
{
    public class ModEntry : Mod
    {
        private ModConfig modConfigs;

        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            modConfigs = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += onLaunched;
            helper.Events.Input.ButtonPressed += this.Events_Input_ButtonPressed;
        }

        private void onLaunched(object sender, GameLaunchedEventArgs e)
        {
            var genericModConfigMenuAPI = Helper.ModRegistry.GetApi<GenericModConfigMenuAPI>("spacechase0.GenericModConfigMenu");

            if (genericModConfigMenuAPI != null)
            {
                genericModConfigMenuAPI.RegisterModConfig(ModManifest, () => modConfigs = new ModConfig(), () => Helper.WriteConfig(modConfigs));

                genericModConfigMenuAPI.RegisterSimpleOption(ModManifest, "Increase zoom or UI", "The keybind that increases the zoom or UI in-game.", () => modConfigs.IncreaseZoomOrUI, (KeybindList val) => modConfigs.IncreaseZoomOrUI = val);
                genericModConfigMenuAPI.RegisterSimpleOption(ModManifest, "Decrease zoom or UI", "The keybind that decreases the zoom or UI in-game.", () => modConfigs.DecreaseZoomOrUI, (KeybindList val) => modConfigs.DecreaseZoomOrUI = val);
                genericModConfigMenuAPI.RegisterSimpleOption(ModManifest, "Hold to change UI", "The keybind that you hold to change UI instead of the zoom.", () => modConfigs.HoldToChangeUIKeys, (KeybindList val) => modConfigs.HoldToChangeUIKeys = val);

                genericModConfigMenuAPI.RegisterSimpleOption(ModManifest, "Suppress controller button", "If your inputs are supressed or not.", () => modConfigs.SuppressControllerButton, (bool val) => modConfigs.SuppressControllerButton = val);

                genericModConfigMenuAPI.RegisterClampedOption(ModManifest, "Zoom level increase", "The amount of Zoom level increase.", () => modConfigs.ZoomLevelIncreaseValue, (float val) => modConfigs.ZoomLevelIncreaseValue = val, 0.01f, 0.50f);
                genericModConfigMenuAPI.RegisterClampedOption(ModManifest, "Zoom level decrease", "The amount of Zoom level decrease.", () => modConfigs.ZoomLevelDecreaseValue, (float val) => modConfigs.ZoomLevelDecreaseValue = val, -0.50f, -0.01f);

                genericModConfigMenuAPI.RegisterClampedOption(ModManifest, "Max zoom out level and UI", "The value of the max zoom out level and UI.", () => modConfigs.MaxZoomOutLevelAndUIValue, (float val) => modConfigs.MaxZoomOutLevelAndUIValue = val, 0.15f, 1);
                genericModConfigMenuAPI.RegisterClampedOption(ModManifest, "Max zoom in level and UI", "The value of the max zoom in level and UI.", () => modConfigs.MaxZoomInLevelAndUIValue, (float val) => modConfigs.MaxZoomInLevelAndUIValue = val, 1, 2.5f);
            }
        }

        private void Events_Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady || !Context.IsPlayerFree) { return; }
            bool wasThePreviousButtonPressSucessfull = false;

            if (modConfigs.HoldToChangeUIKeys.IsDown())
            {
                if (modConfigs.IncreaseZoomOrUI.JustPressed())
                {
                    ChangeUILevel(modConfigs.ZoomLevelIncreaseValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (modConfigs.DecreaseZoomOrUI.JustPressed())
                {
                    ChangeUILevel(modConfigs.ZoomLevelDecreaseValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
            }
            else if (modConfigs.IncreaseZoomOrUI.JustPressed())
            {
                ChangeZoomLevel(modConfigs.ZoomLevelIncreaseValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (modConfigs.DecreaseZoomOrUI.JustPressed())
            {
                ChangeZoomLevel(modConfigs.ZoomLevelDecreaseValue);
                wasThePreviousButtonPressSucessfull = true;
            }

            if (modConfigs.SuppressControllerButton == true && wasThePreviousButtonPressSucessfull == true)
            {
                Helper.Input.Suppress(e.Button);
            }
        }

        private void ChangeZoomLevel(float amount = 0)
        {
            if (!Context.IsSplitScreen)
            {
                //Changes ZoomLevel
                Game1.options.singlePlayerBaseZoomLevel = (float)Math.Round(Game1.options.singlePlayerBaseZoomLevel + amount, 2);

                //Caps Max Zoom In Level
                Game1.options.singlePlayerBaseZoomLevel = Game1.options.singlePlayerBaseZoomLevel >= modConfigs.MaxZoomInLevelAndUIValue ? modConfigs.MaxZoomInLevelAndUIValue : Game1.options.singlePlayerBaseZoomLevel;

                //Caps Max Zoom Out Level
                Game1.options.singlePlayerBaseZoomLevel = Game1.options.singlePlayerBaseZoomLevel <= modConfigs.MaxZoomOutLevelAndUIValue ? modConfigs.MaxZoomOutLevelAndUIValue : Game1.options.singlePlayerBaseZoomLevel;

                //Monitor Current Zoom Level
                //this.Monitor.Log($"{Game1.options.singlePlayerBaseZoomLevel}.", LogLevel.Debug);
            }
            else if (Context.IsSplitScreen)
            {
                //Changes ZoomLevel
                Game1.options.localCoopBaseZoomLevel = (float)Math.Round(Game1.options.localCoopBaseZoomLevel + amount, 2);

                //Caps Max Zoom In Level
                Game1.options.localCoopBaseZoomLevel = Game1.options.localCoopBaseZoomLevel >= modConfigs.MaxZoomInLevelAndUIValue ? modConfigs.MaxZoomInLevelAndUIValue : Game1.options.localCoopBaseZoomLevel;

                //Caps Max Zoom Out Level
                Game1.options.localCoopBaseZoomLevel = Game1.options.localCoopBaseZoomLevel <= modConfigs.MaxZoomOutLevelAndUIValue ? modConfigs.MaxZoomOutLevelAndUIValue : Game1.options.localCoopBaseZoomLevel;
            }
            Program.gamePtr.refreshWindowSettings();
        }

        private void ChangeUILevel(float amount = 0)
        {
            if (!Context.IsSplitScreen)
            {
                //Changes UI Zoom Level
                Game1.options.singlePlayerDesiredUIScale = (float)Math.Round(Game1.options.singlePlayerDesiredUIScale + amount, 2);

                //Caps Max UI Zoom In Level
                Game1.options.singlePlayerDesiredUIScale = Game1.options.singlePlayerDesiredUIScale >= modConfigs.MaxZoomInLevelAndUIValue ? modConfigs.MaxZoomInLevelAndUIValue : Game1.options.singlePlayerDesiredUIScale;

                //Caps Max UI Zoom Out Level
                Game1.options.singlePlayerDesiredUIScale = Game1.options.singlePlayerDesiredUIScale <= modConfigs.MaxZoomOutLevelAndUIValue ? modConfigs.MaxZoomOutLevelAndUIValue : Game1.options.singlePlayerDesiredUIScale;

                //Monitor Current UI Level
                //this.Monitor.Log($"{Game1.options.singlePlayerDesiredUIScale}.", LogLevel.Debug);
            }
            else if (Context.IsSplitScreen)
            {
                //Changes UI Zoom Level
                Game1.options.localCoopDesiredUIScale = (float)Math.Round(Game1.options.localCoopDesiredUIScale + amount, 2);

                //Caps Max UI Zoom In Level
                Game1.options.localCoopDesiredUIScale = Game1.options.localCoopDesiredUIScale >= modConfigs.MaxZoomInLevelAndUIValue ? modConfigs.MaxZoomInLevelAndUIValue : Game1.options.localCoopDesiredUIScale;

                //Caps Max UI Zoom Out Level
                Game1.options.localCoopDesiredUIScale = Game1.options.localCoopDesiredUIScale <= modConfigs.MaxZoomOutLevelAndUIValue ? modConfigs.MaxZoomOutLevelAndUIValue : Game1.options.localCoopDesiredUIScale;
            }

            Program.gamePtr.refreshWindowSettings();
        }
    }

    //Generic Mod Config Menu API
    public interface GenericModConfigMenuAPI
    {
        void RegisterModConfig(IManifest mod, Action revertToDefault, Action saveToFile);

        void UnregisterModConfig(IManifest mod);

        void StartNewPage(IManifest mod, string pageName);

        void OverridePageDisplayName(IManifest mod, string pageName, string displayName);

        void RegisterLabel(IManifest mod, string labelName, string labelDesc);

        void RegisterPageLabel(IManifest mod, string labelName, string labelDesc, string newPage);

        void RegisterParagraph(IManifest mod, string paragraph);

        void RegisterImage(IManifest mod, string texPath, Rectangle? texRect = null, int scale = 4);

        void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<bool> optionGet, Action<bool> optionSet);

        void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<int> optionGet, Action<int> optionSet);

        void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<float> optionGet, Action<float> optionSet);

        void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<string> optionGet, Action<string> optionSet);

        void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<SButton> optionGet, Action<SButton> optionSet);

        void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<KeybindList> optionGet, Action<KeybindList> optionSet);

        void RegisterClampedOption(IManifest mod, string optionName, string optionDesc, Func<int> optionGet, Action<int> optionSet, int min, int max);

        void RegisterClampedOption(IManifest mod, string optionName, string optionDesc, Func<float> optionGet, Action<float> optionSet, float min, float max);

        void RegisterClampedOption(IManifest mod, string optionName, string optionDesc, Func<int> optionGet, Action<int> optionSet, int min, int max, int interval);

        void RegisterClampedOption(IManifest mod, string optionName, string optionDesc, Func<float> optionGet, Action<float> optionSet, float min, float max, float interval);

        void RegisterChoiceOption(IManifest mod, string optionName, string optionDesc, Func<string> optionGet, Action<string> optionSet, string[] choices);

        void RegisterComplexOption(IManifest mod, string optionName, string optionDesc,
                                   Func<Vector2, object, object> widgetUpdate,
                                   Func<SpriteBatch, Vector2, object, object> widgetDraw,
                                   Action<object> onSave);

        void SubscribeToChange(IManifest mod, Action<string, bool> changeHandler);

        void SubscribeToChange(IManifest mod, Action<string, int> changeHandler);

        void SubscribeToChange(IManifest mod, Action<string, float> changeHandler);

        void SubscribeToChange(IManifest mod, Action<string, string> changeHandler);

        void OpenModMenu(IManifest mod);
    }
}