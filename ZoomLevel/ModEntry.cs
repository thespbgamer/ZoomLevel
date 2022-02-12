using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace ZoomLevel
{
    public class ModEntry : Mod
    {
        private ModConfig modConfigs;
        private bool wasToggleUIDone = false;
        private float previousUIValue = 1.0f;

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
                genericModConfigMenuAPI.Register(ModManifest, () => modConfigs = new ModConfig(), () => Helper.WriteConfig(modConfigs));

                genericModConfigMenuAPI.AddSectionTitle(ModManifest, "Keybinds:".ToString   , "All the keybinds that can be added or changed.".ToString);
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => modConfigs.KeybindListHoldToChangeUI, (KeybindList val) => modConfigs.KeybindListHoldToChangeUI = val, "UI hold Key".ToString, "Keybinds that you need to hold to change the UI.".ToString);
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => modConfigs.KeybindListIncreaseZoomOrUI, (KeybindList val) => modConfigs.KeybindListIncreaseZoomOrUI = val, "Zoom or UI Levels Increase".ToString, "Keybinds to Increase Zoom or UI Level.".ToString);
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => modConfigs.KeybindListDecreaseZoomOrUI, (KeybindList val) => modConfigs.KeybindListDecreaseZoomOrUI = val, "Zoom or UI Levels Decrease".ToString, "Keybinds to Decrease Zoom or UI Level.".ToString);
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => modConfigs.KeybindListResetZoomOrUI, (KeybindList val) => modConfigs.KeybindListResetZoomOrUI = val, "Zoom or UI Levels Reset".ToString, "Keybinds that you use to Reset the Zoom or UI Level.".ToString);
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => modConfigs.KeybindListMaxZoomOrUI, (KeybindList val) => modConfigs.KeybindListMaxZoomOrUI = val, "Zoom or UI Max Levels".ToString, "Keybinds to Max the Zoom out or Maximize the UI.".ToString);
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => modConfigs.KeybindListMinZoomOrUI, (KeybindList val) => modConfigs.KeybindListMinZoomOrUI = val, "Zoom or UI Min Levels".ToString, "Keybinds to Max the Zoom in or Minimize the UI.".ToString);
                genericModConfigMenuAPI.AddKeybindList(ModManifest, () => modConfigs.KeybindListToggleUI, (KeybindList val) => modConfigs.KeybindListToggleUI = val, "Toggle UI Visibility".ToString, "Keybinds to toggle the UI Visibility.".ToString);

                genericModConfigMenuAPI.AddSectionTitle(ModManifest, "Values:".ToString, "All the values that changes the Zoom Level and UI Level.".ToString);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => modConfigs.ZoomLevelIncreaseValue, (float val) => modConfigs.ZoomLevelIncreaseValue = val, "Zoom or UI Levels Increase".ToString, "The amount of Zoom or UI Level increase.".ToString, 0.01f, 0.50f,0.01f);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => modConfigs.ZoomLevelDecreaseValue, (float val) => modConfigs.ZoomLevelDecreaseValue = val, "Zoom or UI Levels Decrease".ToString, "The amount of Zoom or UI Level decrease.".ToString, -0.50f, -0.01f, 0.01f);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => modConfigs.MaxZoomOutLevelAndUIValue, (float val) => modConfigs.MaxZoomOutLevelAndUIValue = val, "Zoom or UI Max Level".ToString, "The value of the max Zoom out Level or Max UI.".ToString, 0.15f, 1f, 0.01f);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => modConfigs.MaxZoomInLevelAndUIValue, (float val) => modConfigs.MaxZoomInLevelAndUIValue = val, "Zoom or UI Min  Level".ToString, "The value of the max Zoom in Level or Min UI.".ToString, 1f, 2.5f, 0.01f);
                genericModConfigMenuAPI.AddNumberOption(ModManifest, () => modConfigs.ResetZoomOrUIValue, (float val) => modConfigs.ResetZoomOrUIValue = val, "Zoom or UI Reset Level".ToString, "The value of the Zoom or UI level reset.".ToString, 0.15f, 2.5f, 0.01f);


                genericModConfigMenuAPI.AddSectionTitle(ModManifest, "Other:".ToString, "All the other options that you can change.".ToString);
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => modConfigs.SuppressControllerButton, (bool val) => modConfigs.SuppressControllerButton = val, "Suppress Controller Buttons".ToString, "If your controller inputs are supressed or not.".ToString);
                genericModConfigMenuAPI.AddBoolOption(ModManifest, () => modConfigs.ZoomAndUIControlEverywhere, (bool val) => modConfigs.ZoomAndUIControlEverywhere = val, "Zoom and UI Anywhere".ToString, "If activated you can control your Zoom and UI Levels anywhere.".ToString);
                
            }
        }


        private void Events_Input_ButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady || (!Context.IsPlayerFree && !modConfigs.ZoomAndUIControlEverywhere)) { return; }
            bool wasThePreviousButtonPressSucessfull = false;

            if (modConfigs.KeybindListHoldToChangeUI.IsDown())
            {
                if (modConfigs.KeybindListIncreaseZoomOrUI.JustPressed())
                {
                    ChangeUILevel(modConfigs.ZoomLevelIncreaseValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (modConfigs.KeybindListDecreaseZoomOrUI.JustPressed())
                {
                    ChangeUILevel(modConfigs.ZoomLevelDecreaseValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (modConfigs.KeybindListResetZoomOrUI.JustPressed())
                {
                    ResetUI();
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (modConfigs.KeybindListMaxZoomOrUI.JustPressed())
                {
                    CapUILevel(modConfigs.MaxZoomOutLevelAndUIValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (modConfigs.KeybindListMinZoomOrUI.JustPressed())
                {
                    CapUILevel(modConfigs.MaxZoomInLevelAndUIValue);
                    wasThePreviousButtonPressSucessfull = true;
                }
                else if (modConfigs.KeybindListToggleUI.JustPressed())
                {
                    ToggleUI();
                }
            }
            else if (modConfigs.KeybindListIncreaseZoomOrUI.JustPressed())
            {
                ChangeZoomLevel(modConfigs.ZoomLevelIncreaseValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (modConfigs.KeybindListDecreaseZoomOrUI.JustPressed())
            {
                ChangeZoomLevel(modConfigs.ZoomLevelDecreaseValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (modConfigs.KeybindListResetZoomOrUI.JustPressed())
            {
                ResetZoom();
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (modConfigs.KeybindListMaxZoomOrUI.JustPressed())
            {
                CapZoomLevel(modConfigs.MaxZoomOutLevelAndUIValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (modConfigs.KeybindListMinZoomOrUI.JustPressed())
            {
                CapZoomLevel(modConfigs.MaxZoomInLevelAndUIValue);
                wasThePreviousButtonPressSucessfull = true;
            }
            else if (modConfigs.KeybindListToggleUI.JustPressed())
            {
                ToggleUI();
            }

            if (modConfigs.SuppressControllerButton == true && wasThePreviousButtonPressSucessfull == true)
            {
                Helper.Input.Suppress(e.Button);
            }
        }

        private void ToggleUI()
        {
            float uiValue = 0.0f;
            
            if(wasToggleUIDone == true)
            {
                uiValue = previousUIValue;
            }
            wasToggleUIDone = !wasToggleUIDone;

            if (!Context.IsSplitScreen)
            {
                if (wasToggleUIDone == true)
                {
                    previousUIValue = Game1.options.singlePlayerDesiredUIScale;
                }

                //Changes ZoomLevel
                Game1.options.singlePlayerDesiredUIScale = uiValue;
            }
            else if (Context.IsSplitScreen)
            {
                if (wasToggleUIDone == true)
                {
                    previousUIValue = Game1.options.localCoopDesiredUIScale;
                }

                //Changes ZoomLevel
                Game1.options.localCoopDesiredUIScale = uiValue;
            }
            
            RefreshWindow();
        }

        private void CapZoomLevel(float zoomValue)
        {

            if (!Context.IsSplitScreen)
            {
                //Changes ZoomLevel
                Game1.options.singlePlayerBaseZoomLevel = zoomValue;
            }
            else if (Context.IsSplitScreen)
            {
                //Changes ZoomLevel
                Game1.options.localCoopBaseZoomLevel = zoomValue;
            }
            RefreshWindow();
        }

        private void CapUILevel(float uiValue)
        {

            if (!Context.IsSplitScreen)
            {
                //Changes ZoomLevel
                Game1.options.singlePlayerDesiredUIScale = uiValue;
            }
            else if (Context.IsSplitScreen)
            {
                //Changes ZoomLevel
                Game1.options.localCoopDesiredUIScale = uiValue;
            }
            RefreshWindow();
        }

        private void ResetUI()
        {
            if (!Context.IsSplitScreen)
            {
                Game1.options.singlePlayerDesiredUIScale = modConfigs.ResetZoomOrUIValue;
            }
            else
            {
                Game1.options.localCoopDesiredUIScale = modConfigs.ResetZoomOrUIValue;
            }
            RefreshWindow();
        }

        private void ResetZoom()
        {
            if (!Context.IsSplitScreen)
            {
                Game1.options.singlePlayerBaseZoomLevel = modConfigs.ResetZoomOrUIValue;
            }
            else
            {
                Game1.options.localCoopBaseZoomLevel = modConfigs.ResetZoomOrUIValue;
            }
            RefreshWindow();
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
            RefreshWindow();
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

            RefreshWindow();
        }

        private void RefreshWindow()
        {
            /*
            if (!Context.IsSplitScreen)
            {
                //Monitor Current Zoom Level
                this.Monitor.Log($"{Game1.options.singlePlayerBaseZoomLevel}.", LogLevel.Debug);
                //Monitor Current UI Level
                this.Monitor.Log($"{Game1.options.singlePlayerDesiredUIScale}.", LogLevel.Debug);
            }
            else if (Context.IsSplitScreen)
            {
                //Monitor Current Zoom Level
                this.Monitor.Log($"{Game1.options.localCoopBaseZoomLevel}.", LogLevel.Debug);
                //Monitor Current UI Level
                this.Monitor.Log($"{Game1.options.localCoopDesiredUIScale}.", LogLevel.Debug);
            }
            */
            Program.gamePtr.refreshWindowSettings();
        }
    }


    //Generic Mod Config Menu API
    public interface GenericModConfigMenuAPI
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
