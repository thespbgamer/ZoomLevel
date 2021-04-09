using System;
using Microsoft.Xna.Framework;
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

            helper.Events.Input.ButtonPressed += this.Events_Input_ButtonPressed;
        }

        private void Events_Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady || !Context.IsPlayerFree) { return; }
            bool wasThePreviousButtonPressSucessfull = false;

            if (modConfigs.holdToChangeUIKeys.IsDown())
            {
                if (modConfigs.increaseZoomOrUI.JustPressed())
                {
                    ChangeUILevel(modConfigs.ZoomLevelIncreaseValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (modConfigs.decreaseZoomOrUI.JustPressed())
                {
                    ChangeUILevel(modConfigs.ZoomLevelDecreaseValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
            }
            else if (modConfigs.increaseZoomOrUI.JustPressed())
            {
                ChangeZoomLevel(modConfigs.ZoomLevelIncreaseValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (modConfigs.decreaseZoomOrUI.JustPressed())
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
                Game1.options.singlePlayerBaseZoomLevel = Game1.options.singlePlayerBaseZoomLevel >= modConfigs.MaxZoomInLevelValue ? modConfigs.MaxZoomInLevelValue : Game1.options.singlePlayerBaseZoomLevel;

                //Caps Max Zoom Out Level
                Game1.options.singlePlayerBaseZoomLevel = Game1.options.singlePlayerBaseZoomLevel <= modConfigs.MaxZoomOutLevelValue ? modConfigs.MaxZoomOutLevelValue : Game1.options.singlePlayerBaseZoomLevel;

                //Monitor Current Zoom Level
                //this.Monitor.Log($"{Game1.options.singlePlayerBaseZoomLevel}.", LogLevel.Debug);
            }
            else if (Context.IsSplitScreen)
            {
                //Changes ZoomLevel
                Game1.options.localCoopBaseZoomLevel = (float)Math.Round(Game1.options.localCoopBaseZoomLevel + amount, 2);

                //Caps Max Zoom In Level
                Game1.options.localCoopBaseZoomLevel = Game1.options.localCoopBaseZoomLevel >= modConfigs.MaxZoomInLevelValue ? modConfigs.MaxZoomInLevelValue : Game1.options.localCoopBaseZoomLevel;

                //Caps Max Zoom Out Level
                Game1.options.localCoopBaseZoomLevel = Game1.options.localCoopBaseZoomLevel <= modConfigs.MaxZoomOutLevelValue ? modConfigs.MaxZoomOutLevelValue : Game1.options.localCoopBaseZoomLevel;
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
                Game1.options.singlePlayerDesiredUIScale = Game1.options.singlePlayerDesiredUIScale >= modConfigs.MaxZoomInLevelValue ? modConfigs.MaxZoomInLevelValue : Game1.options.singlePlayerDesiredUIScale;

                //Caps Max UI Zoom Out Level
                Game1.options.singlePlayerDesiredUIScale = Game1.options.singlePlayerDesiredUIScale <= modConfigs.MaxZoomOutLevelValue ? modConfigs.MaxZoomOutLevelValue : Game1.options.singlePlayerDesiredUIScale;

                //Monitor Current UI Level
                //this.Monitor.Log($"{Game1.options.singlePlayerDesiredUIScale}.", LogLevel.Debug);
            }
            else if (Context.IsSplitScreen)
            {
                //Changes UI Zoom Level
                Game1.options.localCoopDesiredUIScale = (float)Math.Round(Game1.options.localCoopDesiredUIScale + amount, 2);

                //Caps Max UI Zoom In Level
                Game1.options.localCoopDesiredUIScale = Game1.options.localCoopDesiredUIScale >= modConfigs.MaxZoomInLevelValue ? modConfigs.MaxZoomInLevelValue : Game1.options.localCoopDesiredUIScale;

                //Caps Max UI Zoom Out Level
                Game1.options.localCoopDesiredUIScale = Game1.options.localCoopDesiredUIScale <= modConfigs.MaxZoomOutLevelValue ? modConfigs.MaxZoomOutLevelValue : Game1.options.localCoopDesiredUIScale;
            }

            Program.gamePtr.refreshWindowSettings();
        }
    }
}