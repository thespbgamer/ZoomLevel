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
        private ModConfig _config;

        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            _config = helper.ReadConfig<ModConfig>();

            helper.Events.Input.ButtonPressed += this.Events_Input_ButtonPressed;
        }

        private void Events_Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (e.Button.TryGetKeyboard(out Keys _))
            {
                if (e.Button == _config.IncreaseZoomKey)
                {
                    ChangeZoomLevel(_config.ZoomLevelIncreaseValue);
                }
                else if (e.Button == _config.DecreaseZoomKey)
                {
                    ChangeZoomLevel(_config.ZoomLevelDecreaseValue);
                }
            }
            else if (e.Button.TryGetController(out Buttons _))
            {
                bool wasPreviousButtonPressZoom = false;

                if (e.Button == _config.IncreaseZoomButton)
                {
                    ChangeZoomLevel(_config.ZoomLevelIncreaseValue);
                    wasPreviousButtonPressZoom = true;
                }
                else if (e.Button == _config.DecreaseZoomButton)
                {
                    ChangeZoomLevel(_config.ZoomLevelDecreaseValue);
                    wasPreviousButtonPressZoom = true;
                }

                if (_config.SuppressControllerButton == true && wasPreviousButtonPressZoom == true)
                {
                    Helper.Input.Suppress(e.Button);
                }
            }
        }

        private void ChangeZoomLevel(float amount = 0)
        {
            //Changes ZoomLevel
            Game1.options.singlePlayerBaseZoomLevel = (float)Math.Round(Game1.options.singlePlayerBaseZoomLevel + amount, 2);

            //Caps Max Zoom In Level
            Game1.options.singlePlayerBaseZoomLevel = Game1.options.singlePlayerBaseZoomLevel >= _config.MaxZoomInLevelValue ? _config.MaxZoomInLevelValue : Game1.options.singlePlayerBaseZoomLevel;

            //Caps Max Zoom Out Level
            Game1.options.singlePlayerBaseZoomLevel = Game1.options.singlePlayerBaseZoomLevel <= _config.MaxZoomOutLevelValue ? _config.MaxZoomOutLevelValue : Game1.options.singlePlayerBaseZoomLevel;

            //this.Monitor.Log($"{Game1.options.singlePlayerBaseZoomLevel}.", LogLevel.Debug);
            Program.gamePtr.refreshWindowSettings();
        }
    }
}