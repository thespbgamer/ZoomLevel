using StardewModdingAPI;
using StardewModdingAPI.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomLevel
{
    internal class ModConfig
    {
        public KeybindList IncreaseZoomOrUI { get; set; } = KeybindList.Parse("OemPeriod, RightStick");
        public KeybindList DecreaseZoomOrUI { get; set; } = KeybindList.Parse("OemComma, LeftStick");
        public KeybindList HoldToChangeUIKeys { get; set; } = KeybindList.Parse("LeftShift, RightShift, LeftTrigger + RightTrigger");
        public KeybindList ResetZoom { get; set; } = KeybindList.Parse("Home");
        public KeybindList ResetUI { get; set; } = KeybindList.Parse("Home");

        public bool SuppressControllerButton { get; set; } = true;
        public bool ZoomAndUIControlEverywhere { get; set; } = false;

        public float ZoomLevelIncreaseValue { get; set; } = 0.05f;
        public float ZoomLevelDecreaseValue { get; set; } = -0.05f;

        public float MaxZoomOutLevelAndUIValue { get; set; } = 0.35f;

        public float MaxZoomInLevelAndUIValue { get; set; } = 2.00f;

        public float ResetUIValue { get; set; } = 1.00f;
        public float ResetZoomValue { get; set; } = 1.00f;
    }
}